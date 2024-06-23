namespace VRCDebug
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.backgroundWorkerRefresh = new System.ComponentModel.BackgroundWorker();
            this.trackBarRefreshRate = new System.Windows.Forms.TrackBar();
            this.labelRefreshRate = new System.Windows.Forms.Label();
            this.panelRefreshRate = new System.Windows.Forms.Panel();
            this.labelRefreshRateGuide = new System.Windows.Forms.Label();
            this.textBoxSearchParameter = new System.Windows.Forms.TextBox();
            this.linkLabelOscLink = new System.Windows.Forms.LinkLabel();
            this.labelParameterCount = new System.Windows.Forms.Label();
            this.labelAvatarID = new System.Windows.Forms.LinkLabel();
            this.buttonToggleTheme = new System.Windows.Forms.Button();
            this.checkBoxBuiltInParams = new System.Windows.Forms.CheckBox();
            this.buttonReloadList = new System.Windows.Forms.Button();
            this.textBoxGuide = new System.Windows.Forms.TextBox();
            this.checkBoxAlwaysOnTop = new System.Windows.Forms.CheckBox();
            this.dataGridViewAvatarParameters = new VRCDebug.CustomDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRefreshRate)).BeginInit();
            this.panelRefreshRate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAvatarParameters)).BeginInit();
            this.SuspendLayout();
            // 
            // backgroundWorkerRefresh
            // 
            this.backgroundWorkerRefresh.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerRefresh_DoWork);
            // 
            // trackBarRefreshRate
            // 
            this.trackBarRefreshRate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarRefreshRate.LargeChange = 1;
            this.trackBarRefreshRate.Location = new System.Drawing.Point(6, 34);
            this.trackBarRefreshRate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackBarRefreshRate.Maximum = 50;
            this.trackBarRefreshRate.Name = "trackBarRefreshRate";
            this.trackBarRefreshRate.Size = new System.Drawing.Size(184, 45);
            this.trackBarRefreshRate.TabIndex = 3;
            this.trackBarRefreshRate.Value = 10;
            this.trackBarRefreshRate.ValueChanged += new System.EventHandler(this.trackBarRefreshRate_ValueChanged);
            // 
            // labelRefreshRate
            // 
            this.labelRefreshRate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelRefreshRate.AutoSize = true;
            this.labelRefreshRate.Location = new System.Drawing.Point(64, 83);
            this.labelRefreshRate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRefreshRate.Name = "labelRefreshRate";
            this.labelRefreshRate.Size = new System.Drawing.Size(66, 20);
            this.labelRefreshRate.TabIndex = 4;
            this.labelRefreshRate.Text = "1000ms";
            // 
            // panelRefreshRate
            // 
            this.panelRefreshRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panelRefreshRate.Controls.Add(this.labelRefreshRateGuide);
            this.panelRefreshRate.Controls.Add(this.labelRefreshRate);
            this.panelRefreshRate.Controls.Add(this.trackBarRefreshRate);
            this.panelRefreshRate.Location = new System.Drawing.Point(5, 404);
            this.panelRefreshRate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelRefreshRate.Name = "panelRefreshRate";
            this.panelRefreshRate.Size = new System.Drawing.Size(194, 108);
            this.panelRefreshRate.TabIndex = 5;
            // 
            // labelRefreshRateGuide
            // 
            this.labelRefreshRateGuide.AutoSize = true;
            this.labelRefreshRateGuide.Location = new System.Drawing.Point(50, 9);
            this.labelRefreshRateGuide.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRefreshRateGuide.Name = "labelRefreshRateGuide";
            this.labelRefreshRateGuide.Size = new System.Drawing.Size(98, 20);
            this.labelRefreshRateGuide.TabIndex = 5;
            this.labelRefreshRateGuide.Text = "Refresh rate";
            // 
            // textBoxSearchParameter
            // 
            this.textBoxSearchParameter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSearchParameter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSearchParameter.Location = new System.Drawing.Point(207, 520);
            this.textBoxSearchParameter.Name = "textBoxSearchParameter";
            this.textBoxSearchParameter.Size = new System.Drawing.Size(975, 26);
            this.textBoxSearchParameter.TabIndex = 6;
            this.textBoxSearchParameter.TextChanged += new System.EventHandler(this.textBoxSearchParameter_TextChanged);
            // 
            // linkLabelOscLink
            // 
            this.linkLabelOscLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabelOscLink.AutoSize = true;
            this.linkLabelOscLink.LinkColor = System.Drawing.Color.DodgerBlue;
            this.linkLabelOscLink.Location = new System.Drawing.Point(69, 526);
            this.linkLabelOscLink.Name = "linkLabelOscLink";
            this.linkLabelOscLink.Size = new System.Drawing.Size(66, 20);
            this.linkLabelOscLink.TabIndex = 7;
            this.linkLabelOscLink.TabStop = true;
            this.linkLabelOscLink.Text = "osc_link";
            this.linkLabelOscLink.Visible = false;
            this.linkLabelOscLink.VisitedLinkColor = System.Drawing.Color.DodgerBlue;
            this.linkLabelOscLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelOscLink_LinkClicked);
            // 
            // labelParameterCount
            // 
            this.labelParameterCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelParameterCount.AutoSize = true;
            this.labelParameterCount.Location = new System.Drawing.Point(23, 379);
            this.labelParameterCount.Name = "labelParameterCount";
            this.labelParameterCount.Size = new System.Drawing.Size(159, 20);
            this.labelParameterCount.TabIndex = 8;
            this.labelParameterCount.Text = "labelParameterCount";
            this.labelParameterCount.Visible = false;
            this.labelParameterCount.MouseEnter += new System.EventHandler(this.labelParameterCount_MouseEnter);
            this.labelParameterCount.MouseLeave += new System.EventHandler(this.labelParameterCount_MouseLeave);
            // 
            // labelAvatarID
            // 
            this.labelAvatarID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAvatarID.AutoSize = true;
            this.labelAvatarID.LinkColor = System.Drawing.Color.DodgerBlue;
            this.labelAvatarID.Location = new System.Drawing.Point(658, 13);
            this.labelAvatarID.Name = "labelAvatarID";
            this.labelAvatarID.Size = new System.Drawing.Size(72, 20);
            this.labelAvatarID.TabIndex = 9;
            this.labelAvatarID.TabStop = true;
            this.labelAvatarID.Text = "AvatarID";
            this.labelAvatarID.Visible = false;
            this.labelAvatarID.VisitedLinkColor = System.Drawing.Color.DodgerBlue;
            this.labelAvatarID.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.labelAvatarID_LinkClicked);
            // 
            // buttonToggleTheme
            // 
            this.buttonToggleTheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonToggleTheme.Location = new System.Drawing.Point(5, 44);
            this.buttonToggleTheme.Name = "buttonToggleTheme";
            this.buttonToggleTheme.Size = new System.Drawing.Size(195, 31);
            this.buttonToggleTheme.TabIndex = 10;
            this.buttonToggleTheme.Text = "Toggle Theme";
            this.buttonToggleTheme.UseVisualStyleBackColor = true;
            this.buttonToggleTheme.Click += new System.EventHandler(this.buttonToggleTheme_Click);
            // 
            // checkBoxBuiltInParams
            // 
            this.checkBoxBuiltInParams.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxBuiltInParams.AutoSize = true;
            this.checkBoxBuiltInParams.Checked = true;
            this.checkBoxBuiltInParams.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxBuiltInParams.Location = new System.Drawing.Point(22, 352);
            this.checkBoxBuiltInParams.Name = "checkBoxBuiltInParams";
            this.checkBoxBuiltInParams.Size = new System.Drawing.Size(161, 24);
            this.checkBoxBuiltInParams.TabIndex = 11;
            this.checkBoxBuiltInParams.Text = "Built-in parameters";
            this.checkBoxBuiltInParams.UseVisualStyleBackColor = true;
            this.checkBoxBuiltInParams.CheckedChanged += new System.EventHandler(this.checkBoxBuiltInParams_CheckedChanged);
            // 
            // buttonReloadList
            // 
            this.buttonReloadList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonReloadList.Location = new System.Drawing.Point(5, 81);
            this.buttonReloadList.Name = "buttonReloadList";
            this.buttonReloadList.Size = new System.Drawing.Size(195, 31);
            this.buttonReloadList.TabIndex = 12;
            this.buttonReloadList.Text = "Reload List";
            this.buttonReloadList.UseVisualStyleBackColor = true;
            this.buttonReloadList.Click += new System.EventHandler(this.buttonReloadList_Click);
            // 
            // textBoxGuide
            // 
            this.textBoxGuide.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxGuide.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxGuide.Enabled = false;
            this.textBoxGuide.Location = new System.Drawing.Point(5, 118);
            this.textBoxGuide.Multiline = true;
            this.textBoxGuide.Name = "textBoxGuide";
            this.textBoxGuide.ReadOnly = true;
            this.textBoxGuide.Size = new System.Drawing.Size(195, 228);
            this.textBoxGuide.TabIndex = 13;
            this.textBoxGuide.TabStop = false;
            // 
            // checkBoxAlwaysOnTop
            // 
            this.checkBoxAlwaysOnTop.AutoSize = true;
            this.checkBoxAlwaysOnTop.Location = new System.Drawing.Point(5, 12);
            this.checkBoxAlwaysOnTop.Name = "checkBoxAlwaysOnTop";
            this.checkBoxAlwaysOnTop.Size = new System.Drawing.Size(130, 24);
            this.checkBoxAlwaysOnTop.TabIndex = 14;
            this.checkBoxAlwaysOnTop.Text = "Always on Top";
            this.checkBoxAlwaysOnTop.UseVisualStyleBackColor = true;
            this.checkBoxAlwaysOnTop.CheckedChanged += new System.EventHandler(this.checkBoxAlwaysOnTop_CheckedChanged);
            // 
            // dataGridViewAvatarParameters
            // 
            this.dataGridViewAvatarParameters.AllowUserToAddRows = false;
            this.dataGridViewAvatarParameters.AllowUserToDeleteRows = false;
            this.dataGridViewAvatarParameters.AllowUserToResizeRows = false;
            this.dataGridViewAvatarParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewAvatarParameters.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewAvatarParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAvatarParameters.DoubleBuffered = true;
            this.dataGridViewAvatarParameters.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dataGridViewAvatarParameters.Location = new System.Drawing.Point(208, 44);
            this.dataGridViewAvatarParameters.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridViewAvatarParameters.MultiSelect = false;
            this.dataGridViewAvatarParameters.Name = "dataGridViewAvatarParameters";
            this.dataGridViewAvatarParameters.RowHeadersVisible = false;
            this.dataGridViewAvatarParameters.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dataGridViewAvatarParameters.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewAvatarParameters.Size = new System.Drawing.Size(975, 468);
            this.dataGridViewAvatarParameters.TabIndex = 0;
            this.dataGridViewAvatarParameters.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewAvatarParameters_CellBeginEdit);
            this.dataGridViewAvatarParameters.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewAvatarParameters_CellEndEdit);
            this.dataGridViewAvatarParameters.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewAvatarParameters_CellMouseEnter);
            this.dataGridViewAvatarParameters.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewAvatarParameters_CellMouseLeave);
            this.dataGridViewAvatarParameters.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewAvatarParameters_ColumnHeaderMouseClick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 567);
            this.Controls.Add(this.checkBoxAlwaysOnTop);
            this.Controls.Add(this.textBoxGuide);
            this.Controls.Add(this.buttonReloadList);
            this.Controls.Add(this.checkBoxBuiltInParams);
            this.Controls.Add(this.buttonToggleTheme);
            this.Controls.Add(this.labelAvatarID);
            this.Controls.Add(this.labelParameterCount);
            this.Controls.Add(this.linkLabelOscLink);
            this.Controls.Add(this.textBoxSearchParameter);
            this.Controls.Add(this.panelRefreshRate);
            this.Controls.Add(this.dataGridViewAvatarParameters);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(550, 606);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VRChat Debug+";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRefreshRate)).EndInit();
            this.panelRefreshRate.ResumeLayout(false);
            this.panelRefreshRate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAvatarParameters)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorkerRefresh;
        private System.Windows.Forms.TrackBar trackBarRefreshRate;
        private System.Windows.Forms.Label labelRefreshRate;
        private System.Windows.Forms.Panel panelRefreshRate;
        private System.Windows.Forms.Label labelRefreshRateGuide;
        private System.Windows.Forms.TextBox textBoxSearchParameter;
        private System.Windows.Forms.LinkLabel linkLabelOscLink;
        private System.Windows.Forms.Label labelParameterCount;
        private CustomDataGridView dataGridViewAvatarParameters;
        private System.Windows.Forms.Button buttonToggleTheme;
        private System.Windows.Forms.LinkLabel labelAvatarID;
        private System.Windows.Forms.CheckBox checkBoxBuiltInParams;
        private System.Windows.Forms.Button buttonReloadList;
        private System.Windows.Forms.TextBox textBoxGuide;
        private System.Windows.Forms.CheckBox checkBoxAlwaysOnTop;
    }
}

