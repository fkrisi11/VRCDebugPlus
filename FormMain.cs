using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VRC.OSCQuery;
using Rug.Osc;

namespace VRCDebug
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        // Networking
        static HttpClient _client = new HttpClient();
        OSCQueryServiceProfile VRChatClient;
        UdpClient udpClient = new UdpClient();
        IPEndPoint vrChatEndpoint;
        bool FindVRC = true;

        // Data
        BindingSource bindingSource = new BindingSource();
        DataTable dataTable;
        Dictionary<string, bool> valueChangedTracker = new Dictionary<string, bool>();
        Dictionary<string, string> previousValues = new Dictionary<string, string>();
        Dictionary<string, ListSortDirection> columnSortDirections = new Dictionary<string, ListSortDirection>();
        bool ShowBuiltins = true;
        string avatarID = "";
        string previousAvatarID = "";
        int intParamCount = 0;
        int floatParamCount = 0;
        int boolParamCount = 0;
        int unknownParamCount = 0;

        // UI
        bool isDragging = false;
        Point dragCursorPoint = new Point(0, 0);
        Point dragFormPoint = new Point(0, 0);
        bool isNightMode = false;
        int RefreshDelayInMs = 1000;
        bool StopRefreshing = false;
        bool PauseRefresh = false;
        EditedCell editedCell;

        struct EditedCell
        {
            public string Name;
            public string PreviousValue;
            public string Type;
            public DataGridViewCell CellReference;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // Default to . as the decimal separator
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            // Load config values
            Size WindowSize = ConfigManager.LoadWindowSize();
            Width = WindowSize.Width;
            Height = WindowSize.Height;

            isNightMode = ConfigManager.LoadIsNightMode();

            if (isNightMode)
                ThemeManager.ApplyNightMode(Controls);
            else
                ThemeManager.ApplyDefaultMode(Controls);

            // Form dragging extension
            foreach (Control c in Controls)
            {
                if (c is Panel || c is Label && !(c is LinkLabel))
                    AddFormDraggingEvent(c);
            }

            AddFormDraggingEvent(labelRefreshRateGuide);
            AddFormDraggingEvent(labelRefreshRate);

            // Initial data setup
            dataTable = new DataTable();

            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Type", typeof(string));
            dataTable.Columns.Add("Value", typeof(string));

            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["Name"] };

            bindingSource.DataSource = dataTable;
            dataGridViewAvatarParameters.DataSource = bindingSource;

            // Allow only the "Value" column to be editable
            foreach (DataGridViewColumn column in dataGridViewAvatarParameters.Columns)
            {
                if (column.Name != "Value")
                    column.ReadOnly = true;
            }

            if (!backgroundWorkerRefresh.IsBusy)
                backgroundWorkerRefresh.RunWorkerAsync();
        }

        // Find VRChat
        public void RunOSCQuery()
        {
            try
            {
                int tcpPort = Extensions.GetAvailableTcpPort();
                int udpPort = Extensions.GetAvailableUdpPort();
                OSCQueryService oscQuery = new OSCQueryServiceBuilder()
                                               .WithTcpPort(tcpPort)
                                               .WithUdpPort(udpPort)
                                               .WithServiceName("VRChat Debug+")
                                               .AdvertiseOSC()
                                               .AdvertiseOSCQuery()
                                               .StartHttpServer()
                                               .Build();

                List<OSCQueryServiceProfile> oscQueryServiceProfiles = oscQuery.GetOSCQueryServices().ToList();
                OSCQueryServiceProfile oscQueryServiceProfile = oscQueryServiceProfiles.FirstOrDefault(x => x.name.Contains("VRChat-Client"));

                if (oscQueryServiceProfile != null)
                {
                    VRChatClient = oscQueryServiceProfile;
                    vrChatEndpoint = new IPEndPoint(VRChatClient.address, 9000);
                    FindVRC = false;
                    Invoke(new Action(() => linkLabelOscLink.Text = "http://127.0.0.1:" + VRChatClient.port.ToString()));
                    Invoke(new Action(() => linkLabelOscLink.Visible = true));
                    Invoke(new Action(() => linkLabelOscLink.Left = panelRefreshRate.Left + panelRefreshRate.Width / 2 - linkLabelOscLink.Width / 2));
                }
            }
            catch (Exception) { }
        }

        #region Data reading
        // Read values from VRChat
        private async void ReadValues()
        {
            if (VRChatClient == null)
            {
                FindVRC = true;
                Invoke(new Action(() =>
                {
                    labelParameterCount.Visible = false;
                    linkLabelOscLink.Visible = false;
                    labelAvatarID.Visible = false;
                }));

                return;
            }

            if (StopRefreshing)
                return;

            Dictionary<string, OSCQueryNode> oscQueryNodes = await GetParameterNodes(VRChatClient.address, VRChatClient.port);

            if (oscQueryNodes == null)
                return;

            try
            {
                if (!StopRefreshing)
                    Invoke(new Action(() => DisplayData(oscQueryNodes, avatarID)));
            }
            catch (Exception) { }
        }

        // Reading parameters
        public async Task<Dictionary<string, OSCQueryNode>> GetParameterNodes(IPAddress ip, int port)
        {
            OSCQueryRootNode rootNode = await GetTree(ip, port);
            if (rootNode == null || rootNode.Contents == null) return null;
            if (!rootNode.Contents.TryGetValue("avatar", out OSCQueryNode avatarNode)) return null;
            if (!avatarNode.Contents.TryGetValue("parameters", out OSCQueryNode parametersNode)) return null;

            Dictionary<string, OSCQueryNode> avatarQueryNodes = new Dictionary<string, OSCQueryNode>(avatarNode.Contents.Count);
            Stack<OSCQueryNode> avatarTodo = new Stack<OSCQueryNode>(avatarNode.Contents.Count);
            avatarTodo.Push(avatarNode);

            // /avatar reading
            while (avatarTodo.Count > 0)
            {
                OSCQueryNode oSCQueryNode = avatarTodo.Pop();
                if (oSCQueryNode != avatarNode)
                {
                    avatarQueryNodes.Add(oSCQueryNode.FullPath.Remove(0, "/avatar".Length), oSCQueryNode);
                }

                if (oSCQueryNode.Contents == null) continue;

                foreach (var kvp in oSCQueryNode.Contents)
                {
                    avatarTodo.Push(kvp.Value);

                    if (kvp.Key.Contains("change"))
                    {
                        // Getting AvatarID
                        avatarID = kvp.Value.Value.Length > 0 ? kvp.Value.Value[0].ToString() : "";
                    }
                }
            }

            // /avatar/parameter reading
            Dictionary<string, OSCQueryNode> oscQueryNodes = new Dictionary<string, OSCQueryNode>(parametersNode.Contents.Count);
            Stack<OSCQueryNode> todo = new Stack<OSCQueryNode>(parametersNode.Contents.Count);
            todo.Push(parametersNode);

            while (todo.Count > 0)
            {
                OSCQueryNode oscQueryNode = todo.Pop();

                if (oscQueryNode != parametersNode)
                {
                    // Collect nodes without their full path
                    oscQueryNodes.Add(oscQueryNode.FullPath.Remove(0, "/avatar/parameters/".Length), oscQueryNode);
                }

                if (oscQueryNode.Contents == null) continue;

                foreach (var kvp in oscQueryNode.Contents)
                {
                    // Collect node values
                    todo.Push(kvp.Value);
                }
            }

            return oscQueryNodes;
        }

        // Get root node
        public static async Task<OSCQueryRootNode> GetTree(IPAddress ip, int port)
        {
            try
            {
                return await GetOSCTree(ip, port);
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
                return null;
            }
        }

        // Taken from the Extension class, to be able to add exception handling
        public static async Task<OSCQueryRootNode> GetOSCTree(IPAddress ip, int port)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"http://{ip}:{port}/");

                if (!response.IsSuccessStatusCode)
                    return new OSCQueryRootNode();

                string oscTreeString = await response.Content.ReadAsStringAsync();
                OSCQueryRootNode oscTree = OSCQueryRootNode.FromString(oscTreeString);

                return oscTree;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new OSCQueryRootNode();
            }
        }

        private void backgroundWorkerRefresh_DoWork(object sender, DoWorkEventArgs e)
        {
            // StopRefreshing true --> Closing form
            while (!StopRefreshing)
            {
                if (FindVRC)
                {
                    while (VRChatClient == null)
                    {
                        if (!StopRefreshing)
                        {
                            Invoke(new Action(() =>
                            {
                                labelParameterCount.Visible = false;
                                linkLabelOscLink.Visible = false;
                                labelAvatarID.Visible = false;
                            }));

                            // Try to connect to VRC
                            RunOSCQuery();
                            Thread.Sleep(2000);
                        }
                    }
                }

                // Keep refreshing data while
                // not paused, and connected to VRC
                if (!PauseRefresh && !FindVRC)
                {
                    if (!StopRefreshing)
                    {
                        ReadValues();
                        Thread.Sleep(RefreshDelayInMs);
                    }
                }
            }
        }

        private void trackBarRefreshRate_ValueChanged(object sender, EventArgs e)
        {
            if (trackBarRefreshRate.Value == trackBarRefreshRate.Minimum)
            {
                RefreshDelayInMs = 1000;
                labelRefreshRate.Text = "Paused";
                PauseRefresh = true;
            }
            else
            {
                RefreshDelayInMs = trackBarRefreshRate.Value * 100;
                labelRefreshRate.Text = RefreshDelayInMs + " ms";
                PauseRefresh = false;
            }

            labelRefreshRate.Left = panelRefreshRate.Width / 2 - labelRefreshRate.Width / 2;
        }
        #endregion Data reading

        #region Data sending
        private void dataGridViewAvatarParameters_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Don't update while editing
            PauseRefresh = true;

            // Set editing cell forecolor
            dataGridViewAvatarParameters.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = isNightMode ? ThemeManager.NightGridHeaderForegroundColor : ThemeManager.DefaultGridHeaderForegroundColor;

            // Save data about the current row
            DataGridViewRow currentRow = dataGridViewAvatarParameters.Rows[dataGridViewAvatarParameters.CurrentCell.RowIndex];
            editedCell = new EditedCell()
            {
                Name = currentRow.Cells["Name"].Value.ToString(),
                PreviousValue = currentRow.Cells["Value"].Value.ToString().Trim().ToLower(),
                Type = currentRow.Cells["Type"].Value.ToString(),
                CellReference = dataGridViewAvatarParameters.Rows[e.RowIndex].Cells[e.ColumnIndex]
            };

            // If value is bool, toggle it and stop editing
            if (editedCell.Type == "Bool")
            {
                if (editedCell.PreviousValue.ToLower() == "false")
                    dataGridViewAvatarParameters.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "True";
                else
                    dataGridViewAvatarParameters.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "False";

                e.Cancel = true;
                dataGridViewAvatarParameters_CellEndEdit(sender, new DataGridViewCellEventArgs(e.ColumnIndex, e.RowIndex));
            }
        }

        private void dataGridViewAvatarParameters_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Resume updating
            PauseRefresh = false;

            if (VRChatClient == null)
            {
                Invoke(new Action(() =>
                {
                    labelParameterCount.Visible = false;
                    linkLabelOscLink.Visible = false;
                    labelAvatarID.Visible = false;
                }));

                return;
            }

            if (StopRefreshing)
                return;


            // Mark parameter as changed
            DataGridViewCell nameCell = dataGridViewAvatarParameters.Rows[e.RowIndex].Cells["Name"];
            DataGridViewCell valueCell = dataGridViewAvatarParameters.Rows[e.RowIndex].Cells["Value"];
            string key = nameCell.Value.ToString();
            string newValue = valueCell.Value.ToString();

            if (previousValues.ContainsKey(key))
                previousValues[key] = newValue;
            else
                previousValues.Add(key, newValue);

            valueChangedTracker[key] = true;


            // OSC Data sending
            string valueToSend = editedCell.CellReference.Value.ToString().Trim().ToLower();

            // Not sending, as the value didn't change
            if (editedCell.PreviousValue.Trim().ToLower() == valueToSend)
                return;

            bool regexMatch = false;

            byte[] messageBytes;

            switch (editedCell.Type)
            {
                case "Bool":
                    if (valueToSend == "t" || valueToSend == "true" || valueToSend == "1" || valueToSend == "1.0")
                        messageBytes = new OscMessage("/avatar/parameters/" + editedCell.Name, new object[] { true }).ToByteArray();
                    else
                        messageBytes = new OscMessage("/avatar/parameters/" + editedCell.Name, new object[] { false }).ToByteArray();
                    break;
                case "Float":
                    regexMatch = Regex.IsMatch(valueToSend, @"^[\d,.\-]+$");
                    if (!regexMatch)
                        messageBytes = new OscMessage("/avatar/parameters/" + editedCell.Name, new object[] { 0.0f }).ToByteArray();
                    else
                        messageBytes = new OscMessage("/avatar/parameters/" + editedCell.Name, new object[] { float.Parse(valueToSend.Replace(',', '.')) }).ToByteArray();
                    break;
                case "Integer":
                    regexMatch = Regex.IsMatch(valueToSend, @"^[\d]+$");
                    if (!regexMatch)
                        messageBytes = new OscMessage("/avatar/parameters/" + editedCell.Name, new object[] { 0 }).ToByteArray();
                    else
                        messageBytes = new OscMessage("/avatar/parameters/" + editedCell.Name, new object[] { int.Parse(valueToSend) }).ToByteArray();
                    break;
                default:
                    // Unknown type
                    messageBytes = new OscMessage("/avatar/parameters/" + editedCell.Name, new object[] { valueToSend }).ToByteArray();
                    break;
            }

            udpClient.Send(messageBytes, messageBytes.Length, vrChatEndpoint);

            // Refresh data after sending OSC packet
            ReadValues();
        }
        #endregion Data sending

        #region Link labels
        private void linkLabelOscLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabelOscLink.Text);
        }

        private void labelAvatarID_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://vrchat.com/home/avatar/" + labelAvatarID.Text);
        }
        #endregion Link labels

        #region Sorting
        private void SortDataGridView(string primaryColumn, ListSortDirection primaryDirection, string secondaryColumn)
        {
            DataView dataView = dataTable.DefaultView;
            string primaryDirectionStr = primaryDirection == ListSortDirection.Ascending ? "ASC" : "DESC";
            dataView.Sort = $"{primaryColumn} {primaryDirectionStr}, {secondaryColumn} ASC";
        }

        private void ResetSorting()
        {
            DataView dataView = dataTable.DefaultView;
            dataView.Sort = "";
            columnSortDirections.Clear();

            foreach (DataGridViewColumn column in dataGridViewAvatarParameters.Columns)
            {
                column.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
        }

        private void dataGridViewAvatarParameters_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn clickedColumn = dataGridViewAvatarParameters.Columns[e.ColumnIndex];
            string columnName = clickedColumn.Name;

            if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle)
            {
                ResetSorting();
                return;
            }

            // Determine the new sort direction
            if (!columnSortDirections.TryGetValue(columnName, out ListSortDirection sortDirection))
                sortDirection = ListSortDirection.Ascending;
            else
                sortDirection = sortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

            // Update the dictionary with the new sort direction
            columnSortDirections[columnName] = sortDirection;

            // Update the sort glyph
            clickedColumn.HeaderCell.SortGlyphDirection = sortDirection == ListSortDirection.Ascending ? SortOrder.Ascending : SortOrder.Descending;

            // Sort
            SortDataGridView(columnName, sortDirection, "Name");
        }
        #endregion Sorting

        #region Form moving events
        private void FormMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = Location;
            }

            if (e.Button == MouseButtons.Middle)
            {
                Size = new Size(1216, 606);
            }
        }

        private void AddFormDraggingEvent(Control control)
        {
            control.MouseDown += FormMain_MouseDown;
            control.MouseMove += FormMain_MouseMove;
            control.MouseUp += FormMain_MouseUp;
        }

        private void FormMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void FormMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                isDragging = false;
        }
        #endregion Form moving events

        #region Night mode toggle
        private void buttonToggleTheme_Click(object sender, EventArgs e) { ToggleNightMode(); }

        private void ToggleNightMode()
        {
            isNightMode = !isNightMode;
            ConfigManager.SaveIsNightMode(isNightMode);

            if (isNightMode)
                ThemeManager.ApplyNightMode(Controls);
            else
                ThemeManager.ApplyDefaultMode(Controls);

            // Refresh data to reflect theme change
            ReadValues();
        }
        #endregion Night mode toggle

        #region Other
        private void checkBoxBuiltInParams_CheckedChanged(object sender, EventArgs e)
        {
            ShowBuiltins = checkBoxBuiltInParams.Checked;

            // Refresh data to add the builtins back into the list
            ReadValues();
        }

        private void buttonReloadList_Click(object sender, EventArgs e)
        {
            dataTable.Clear();
            labelParameterCount.Text = "0 Parameters";
            labelAvatarID.Text = "";
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopRefreshing = true;

            try
            {
                ConfigManager.SaveWindowSize(Width, Height);
                udpClient?.Dispose();
            }
            catch (Exception) { }
        }

        private void textBoxSearchParameter_TextChanged(object sender, EventArgs e)
        {
            string searchValue = textBoxSearchParameter.Text;

            if (string.IsNullOrWhiteSpace(searchValue))
            {
                if (dataGridViewAvatarParameters.DataSource is BindingSource bindingSourceLocal)
                    bindingSourceLocal.RemoveFilter();

                return;
            }

            // Split the search terms by space or comma
            string[] searchTerms = searchValue.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            List<string> filterParts = new List<string>();

            foreach (string term in searchTerms)
            {
                // Make \ and / interchangeable
                string normalizedSearchValue = term.Replace("\\", "/").Replace("'", "''");
                string normalizedSearchValueWithBackslash = normalizedSearchValue.Replace("/", "\\");

                if ((term.StartsWith("\"") && term.EndsWith("\"")) || (term.StartsWith("'") && term.EndsWith("'")))
                {
                    // Exact search
                    string exactValue = normalizedSearchValue.Trim('"', '\'');
                    string exactValueWithBackslash = normalizedSearchValueWithBackslash.Trim('"', '\'');
                    filterParts.Add($"Name = '{exactValue}' OR Type = '{exactValue}' OR Value = '{exactValue}' OR " +
                                    $"Name = '{exactValueWithBackslash}' OR Type = '{exactValueWithBackslash}' OR Value = '{exactValueWithBackslash}'");
                }
                else
                {
                    // Partial search
                    filterParts.Add($"Name LIKE '%{normalizedSearchValue}%' OR Type LIKE '%{normalizedSearchValue}%' OR Value LIKE '%{normalizedSearchValue}%' OR " +
                                    $"Name LIKE '%{normalizedSearchValueWithBackslash}%' OR Type LIKE '%{normalizedSearchValueWithBackslash}%' OR Value LIKE '%{normalizedSearchValueWithBackslash}%'");
                }
            }

            string filterExpression = string.Join(" AND ", filterParts);

            if (dataGridViewAvatarParameters.DataSource is BindingSource bindingSource)
                bindingSource.Filter = filterExpression;

            // Refresh data to have proper colors immediately
            ReadValues();
        }

        // Get info about name cell
        private void dataGridViewAvatarParameters_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                DataGridViewCell cell = ((DataGridView)sender)[e.ColumnIndex, e.RowIndex];
                textBoxGuide.Text = GetBuiltinParameterDescription(cell);
            }
        }

        private void dataGridViewAvatarParameters_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            textBoxGuide.Text = "";
        }

        // Get info about built-in parameters
        private string GetBuiltinParameterDescription(DataGridViewCell cell)
        {
            if (cell.Value != null)
            {
                string helperText = "";

                if (!BuiltinParams.List.ContainsKey(cell.Value.ToString()) && valueChangedTracker.ContainsKey(cell.Value.ToString()) && !valueChangedTracker[cell.Value.ToString()])
                    helperText = "\r\n\r\nThis parameter has to update once to show its real value";

                if (BuiltinParams.List.ContainsKey(cell.Value.ToString()))
                    helperText += "\r\n\r\nThis is a built-in parameter that can't be changed";

                if (BuiltinParams.List.ContainsKey(cell.Value.ToString()))
                    return BuiltinParams.List[cell.Value.ToString()] + helperText;

                return cell.Value.ToString() + helperText;
            }
            return "";
        }

        // Display parameter count by type
        private void labelParameterCount_MouseEnter(object sender, EventArgs e)
        {
            if (unknownParamCount == 0)
            {
                textBoxGuide.Text = intParamCount.ToString() + " Int\r\n" +
                    floatParamCount.ToString() + " Float\r\n" +
                    boolParamCount.ToString() + " Bool";
            }
            else
            {
                textBoxGuide.Text = intParamCount.ToString() + " Int\r\n" +
                    floatParamCount.ToString() + " Float\r\n" +
                    boolParamCount.ToString() + " Bool\r\n" +
                    unknownParamCount.ToString() + " Unknown";
            }
        }

        private void labelParameterCount_MouseLeave(object sender, EventArgs e)
        {
            textBoxGuide.Text = "";
        }

        // Debug logging
        private void Log(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
        }
        #endregion Other

        public void DisplayData(Dictionary<string, OSCQueryNode> keyValuePairs, string AvatarID)
        {
            if (InvokeRequired)
            {
                try
                {
                    if (!StopRefreshing)
                        Invoke(new Action(() => DisplayData(keyValuePairs, AvatarID)));
                }
                catch (Exception) { }

                return;
            }

            // Check if AvatarID has changed
            bool isAvatarIDChanged = AvatarID != previousAvatarID;

            if (isAvatarIDChanged)
            {
                // Clear the data table and value change tracker
                dataTable.Clear();
                valueChangedTracker.Clear();
                previousValues.Clear();
                previousAvatarID = AvatarID;
            }

            // Save the current scroll position and selected rows
            int firstDisplayedScrollingRowIndex = dataGridViewAvatarParameters.FirstDisplayedScrollingRowIndex;
            List<int> selectedRows = dataGridViewAvatarParameters.SelectedRows.Cast<DataGridViewRow>().Select(r => r.Index).ToList();

            // Suspend updates to the DataGridView
            dataGridViewAvatarParameters.SuspendLayout();
            dataTable.BeginLoadData();

            // Create a copy of the dictionary for safe iteration
            Dictionary<string, OSCQueryNode> keyValuePairsCopy = new Dictionary<string, OSCQueryNode>(keyValuePairs);

            // Create dictionaries for fast lookups
            Dictionary<string, DataRow> existingRows = dataTable.AsEnumerable().ToDictionary(row => row.Field<string>("Name"));
            Dictionary<string, DataGridViewRow> dataGridViewRowDict = dataGridViewAvatarParameters.Rows.Cast<DataGridViewRow>()
                                                                       .ToDictionary(r => r.Cells["Name"].Value.ToString());

            intParamCount = 0;
            floatParamCount = 0;
            boolParamCount = 0;
            unknownParamCount = 0;

            // Update existing rows and add new rows
            foreach (var kvp in keyValuePairsCopy)
            {
                if (kvp.Key != null && kvp.Value != null && kvp.Value.OscType != null && kvp.Value.Value != null)
                {
                    string name = kvp.Key;
                    string type = kvp.Value.OscType.ToLower();
                    string value = "";

                    switch (type)
                    {
                        case "i":
                            type = "Integer";
                            value = kvp.Value.Value[0].ToString();

                            if (!BuiltinParams.List.ContainsKey(name))
                                intParamCount++;
                            else
                            {
                                if (ShowBuiltins)
                                    intParamCount++;
                            }
                            break;
                        case "f":
                            type = "Float";
                            value = Math.Round(Convert.ToDecimal(kvp.Value.Value[0]), 4).ToString("F4");

                            if (!BuiltinParams.List.ContainsKey(name))
                                floatParamCount++;
                            else
                            {
                                if (ShowBuiltins)
                                    floatParamCount++;
                            }
                            break;
                        case "t":
                            type = "Bool";
                            value = kvp.Value.Value[0].ToString();

                            if (!BuiltinParams.List.ContainsKey(name))
                                boolParamCount++;
                            else
                            {
                                if (ShowBuiltins)
                                    boolParamCount++;
                            }
                            break;
                        default:
                            value = kvp.Value.Value[0].ToString();

                            if (!BuiltinParams.List.ContainsKey(name))
                                unknownParamCount++;
                            else
                            {
                                if (ShowBuiltins)
                                    unknownParamCount++;
                            }
                            break;
                    }

                    if (existingRows.TryGetValue(name, out DataRow row))
                    {
                        // Check for changes in values
                        if (!previousValues.ContainsKey(name))
                            previousValues[name] = row["Value"].ToString();

                        if (row["Value"].ToString() != value)
                            valueChangedTracker[name] = true;

                        // Update existing row and track value changes
                        row["Type"] = type;
                        row["Value"] = value;
                        existingRows.Remove(name); // Mark this parameter as handled
                    }
                    else
                    {
                        if (!BuiltinParams.List.ContainsKey(name) || ShowBuiltins)
                        {
                            dataTable.Rows.Add(name, type, value);
                            previousValues[name] = value;
                            valueChangedTracker[name] = false;
                        }
                    }

                    // Set up ReadOnly values
                    if (dataGridViewRowDict.TryGetValue(name, out DataGridViewRow dataGridViewRow))
                    {
                        bool isReadOnly = (kvp.Value.Access == Attributes.AccessValues.ReadOnly);
                        dataGridViewRow.ReadOnly = isReadOnly;

                        if (isReadOnly)
                        {
                            if (isNightMode)
                                dataGridViewRow.Cells["Value"].Style.BackColor = Color.FromArgb(90, 90, 90);
                            else
                                dataGridViewRow.Cells["Value"].Style.BackColor = Color.LightGray;
                        }
                    }
                }
            }

            // Rows that were not updated and are in BuiltinParams.List are removed if ShowBuiltins is false
            if (!ShowBuiltins)
            {
                List<DataRow> rowsToRemove = dataTable.AsEnumerable()
                                                      .Where(row => BuiltinParams.List.ContainsKey(row.Field<string>("Name")))
                                                      .ToList();

                foreach (var row in rowsToRemove)
                {
                    dataTable.Rows.Remove(row);
                }
            }

            // Display data changes on the UI
            dataTable.EndLoadData();
            dataGridViewAvatarParameters.ResumeLayout();

            // Refresh once to update the selected cell's value
            dataGridViewAvatarParameters.Refresh();

            // Cell colors
            foreach (DataGridViewRow row in dataGridViewAvatarParameters.Rows)
            {
                string name = row.Cells["Name"].Value.ToString();

                // Non built-ins are yellow by default, until their value changed
                if (!BuiltinParams.List.ContainsKey(name) && valueChangedTracker.ContainsKey(name) && !valueChangedTracker[name])
                    row.Cells["Name"].Style.ForeColor = isNightMode ? Color.Yellow : Color.Goldenrod;
                else
                    row.Cells["Name"].Style.ForeColor = isNightMode ? Color.White : Color.Black;

                // Bools
                if (row.Cells["Type"].Value.ToString() == "Bool")
                {
                    if (row.Cells["Value"].Value.ToString() == "True")
                    {
                        // True color
                        if (isNightMode)
                            row.Cells["Value"].Style.ForeColor = Color.LimeGreen;
                        else
                            row.Cells["Value"].Style.ForeColor = Color.FromArgb(0, 190, 0);
                    }
                    else
                    {
                        // False color
                        if (isNightMode)
                            row.Cells["Value"].Style.ForeColor = Color.FromArgb(255, 80, 80);
                        else
                            row.Cells["Value"].Style.ForeColor = Color.Red;
                    }
                }
                else
                {
                    // Every other type
                    if (isNightMode)
                        row.Cells["Value"].Style.ForeColor = Color.White;
                    else
                        row.Cells["Value"].Style.ForeColor = Color.Black;
                }

                // Types
                if (isNightMode)
                    row.Cells["Type"].Style.ForeColor = Color.White;
                else
                    row.Cells["Type"].Style.ForeColor = Color.Black;
            }

            // Parameter count
            labelParameterCount.Text = dataGridViewAvatarParameters.RowCount.ToString() + " Parameters";
            labelParameterCount.Left = panelRefreshRate.Left + panelRefreshRate.Width / 2 - labelParameterCount.Width / 2;
            labelParameterCount.Visible = true;

            // AvatarID
            labelAvatarID.Text = AvatarID;
            labelAvatarID.Left = dataGridViewAvatarParameters.Left + dataGridViewAvatarParameters.Width / 2 - labelAvatarID.Width / 2;
            labelAvatarID.Visible = true;

            // Restore scroll position
            if (firstDisplayedScrollingRowIndex >= 0 && firstDisplayedScrollingRowIndex < dataGridViewAvatarParameters.RowCount)
                dataGridViewAvatarParameters.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex;

            // Restore selection
            foreach (int rowIndex in selectedRows)
            {
                if (rowIndex >= 0 && rowIndex < dataGridViewAvatarParameters.RowCount)
                    dataGridViewAvatarParameters.Rows[rowIndex].Selected = true;
            }
        }
    }
}
