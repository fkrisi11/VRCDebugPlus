using System.Drawing;
using System.Windows.Forms;

namespace VRCDebug
{
    public static class ThemeManager
    {
        public static Color NightBackgroundColor = Color.FromArgb(45, 45, 48);
        public static Color NightForegroundColor = Color.White;
        public static Color NightButtonBackgroundColor = Color.FromArgb(63, 63, 70);
        public static Color NightButtonForegroundColor = Color.White;
        public static Color NightGridAlternatingRowColor = Color.FromArgb(50, 50, 55);
        public static Color NightGridBackgroundColor = Color.FromArgb(45, 45, 48);
        public static Color NightGridHeaderBackgroundColor = Color.FromArgb(30, 30, 30);
        public static Color NightGridHeaderForegroundColor = Color.White;

        public static Color DefaultBackgroundColor = SystemColors.Control;
        public static Color DefaultForegroundColor = SystemColors.ControlText;
        public static Color DefaultButtonBackgroundColor = SystemColors.Control;
        public static Color DefaultButtonForegroundColor = SystemColors.ControlText;
        public static Color DefaultGridAlternatingRowColor = Color.White;
        public static Color DefaultGridBackgroundColor = SystemColors.Window;
        public static Color DefaultGridHeaderBackgroundColor = SystemColors.Control;
        public static Color DefaultGridHeaderForegroundColor = SystemColors.ControlText;

        public static void ApplyNightMode(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                switch (control)
                {
                    case Button button:
                        button.BackColor = NightButtonBackgroundColor;
                        button.ForeColor = NightButtonForegroundColor;
                        break;
                    case Label label:
                        label.ForeColor = NightForegroundColor;
                        break;
                    case TextBox textBox:
                        textBox.BackColor = NightBackgroundColor;
                        textBox.ForeColor = NightForegroundColor;
                        break;
                    case CheckBox checkBox:
                        checkBox.BackColor = NightBackgroundColor;
                        checkBox.ForeColor = NightForegroundColor;
                        break;
                    case CustomDataGridView grid:
                        grid.BackgroundColor = NightGridBackgroundColor;
                        grid.DefaultCellStyle.BackColor = NightBackgroundColor;
                        grid.DefaultCellStyle.ForeColor = NightForegroundColor;
                        grid.AlternatingRowsDefaultCellStyle.BackColor = NightGridAlternatingRowColor;
                        grid.AlternatingRowsDefaultCellStyle.ForeColor = NightForegroundColor;
                        grid.ColumnHeadersDefaultCellStyle.BackColor = NightGridHeaderBackgroundColor;
                        grid.ColumnHeadersDefaultCellStyle.ForeColor = NightGridHeaderForegroundColor;
                        grid.EnableHeadersVisualStyles = false;
                        break;
                }

                if (control.HasChildren)
                    ApplyNightMode(control.Controls);
            }

            if (controls.Owner != null)
                controls.Owner.BackColor = NightBackgroundColor;
        }

        public static void ApplyDefaultMode(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                switch (control)
                {
                    case Button button:
                        button.BackColor = DefaultButtonBackgroundColor;
                        button.ForeColor = DefaultButtonForegroundColor;
                        break;
                    case Label label:
                        label.ForeColor = DefaultForegroundColor;
                        break;
                    case TextBox textBox:
                        textBox.BackColor = DefaultBackgroundColor;
                        textBox.ForeColor = DefaultForegroundColor;
                        break;
                    case CheckBox checkBox:
                        checkBox.BackColor = DefaultBackgroundColor;
                        checkBox.ForeColor = DefaultForegroundColor;
                        break;
                    case CustomDataGridView grid:
                        grid.BackgroundColor = DefaultGridBackgroundColor;
                        grid.DefaultCellStyle.BackColor = DefaultBackgroundColor;
                        grid.DefaultCellStyle.ForeColor = DefaultForegroundColor;
                        grid.AlternatingRowsDefaultCellStyle.BackColor = DefaultGridAlternatingRowColor;
                        grid.AlternatingRowsDefaultCellStyle.ForeColor = DefaultForegroundColor;
                        grid.ColumnHeadersDefaultCellStyle.BackColor = DefaultGridHeaderBackgroundColor;
                        grid.ColumnHeadersDefaultCellStyle.ForeColor = DefaultGridHeaderForegroundColor;
                        grid.EnableHeadersVisualStyles = false;
                        break;
                }

                if (control.HasChildren)
                    ApplyDefaultMode(control.Controls);
            }

            if (controls.Owner != null)
                controls.Owner.BackColor = DefaultBackgroundColor;
        }
    }
}
