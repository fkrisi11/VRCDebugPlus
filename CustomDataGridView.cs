using System.ComponentModel;
using System.Windows.Forms;

namespace VRCDebug
{
    public class CustomDataGridView : DataGridView
    {
        [Category("Behavior")]
        [Description("Gets or sets a value indicating whether to use double buffering to reduce flicker.")]
        public new bool DoubleBuffered
        {
            get { return base.DoubleBuffered; }
            set { base.DoubleBuffered = value; }
        }

        public CustomDataGridView()
        {

        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (IsCurrentCellInEditMode)
                    EndEdit(); // Ensure CellEndEdit is fired
                else
                    BeginEdit(true);

                return true;
            }

            if (keyData == Keys.Space)
            {
                BeginEdit(true);
                return true;
            }

            if (keyData == Keys.F3)
            {
                BeginEdit(true);
                return true;
            }

            return base.ProcessDialogKey(keyData);
        }

        protected override bool ProcessDataGridViewKey(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (IsCurrentCellInEditMode)
                    EndEdit(); // Ensure CellEndEdit is fired
                else
                    BeginEdit(true);

                return true;
            }

            if (e.KeyCode == Keys.Space)
            {
                BeginEdit(true);
                return true;
            }

            if (e.KeyCode == Keys.F3)
            {
                BeginEdit(true);
                return true;
            }

            return base.ProcessDataGridViewKey(e);
        }

        // The mouse events let the user double-click a cell that is not
        // yet selected, to enter edit mode with it immediately
        //
        // At the same time, if a cell is already selected, and
        // it gets clicked once more, it will not enter edit mode
        // it will only enter edit mode with a double-click
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            var hitTestInfo = HitTest(e.X, e.Y);
            if (hitTestInfo.Type == DataGridViewHitTestType.Cell && hitTestInfo.RowIndex >= 0 && hitTestInfo.ColumnIndex >= 0)
            {
                DataGridViewCell clickedCell = this[hitTestInfo.ColumnIndex, hitTestInfo.RowIndex];
                CurrentCell = clickedCell;
                BeginEdit(true);
            }
            else
                base.OnMouseDoubleClick(e);
        }

        protected override void OnCellClick(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell clickedCell = this[e.ColumnIndex, e.RowIndex];

                if (CurrentCell != clickedCell)
                    CurrentCell = clickedCell;
            }

            base.OnCellClick(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            var hitTestInfo = HitTest(e.X, e.Y);

            if (hitTestInfo.Type == DataGridViewHitTestType.Cell && hitTestInfo.RowIndex >= 0 && hitTestInfo.ColumnIndex >= 0)
            {
                DataGridViewCell clickedCell = this[hitTestInfo.ColumnIndex, hitTestInfo.RowIndex];

                if (e.Button == MouseButtons.Right)
                {
                    CurrentCell = clickedCell;
                    BeginEdit(true);
                }
                else if (CurrentCell == clickedCell)
                    return;
            }

            base.OnMouseClick(e);
        }
    }
}
