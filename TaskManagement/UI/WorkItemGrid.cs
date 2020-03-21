using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.ViewModel;

namespace TaskManagement.UI
{
    public class WorkItemGrid : FreeGridControl.GridControl
    {
        private ViewData _viewData;

        public WorkItemGrid() { }

        private void WorkItemGrid_OnDrawCell(object sender, FreeGridControl.DrawEventArgs e)
        {
            var memberIndex = e.ColIndex - this.FixedCols;
            if (0 <= memberIndex)
            {
                var member = _viewData.GetFilteredMembers().ElementAt(memberIndex);
                if (e.RowIndex == 0 && this.FixedCols <= e.ColIndex)
                {
                    e.Graphics.DrawString(member.Company, this.Font, Brushes.Black, e.Rect);
                }
                if (e.RowIndex == 1 && this.FixedCols <= e.ColIndex)
                {
                    e.Graphics.DrawString(member.DisplayName, this.Font, Brushes.Black, e.Rect);
                }
            }

            var dayIndex = e.RowIndex - this.FixedRows;
            if (0 <= dayIndex)
            {
                var day = _viewData.GetFilteredDays().ElementAt(dayIndex);
                if (this.FixedRows <= e.RowIndex && e.ColIndex == 0)
                {
                    e.Graphics.DrawString(day.Year.ToString(), this.Font, Brushes.Black, e.Rect);
                }
                if (this.FixedRows <= e.RowIndex && e.ColIndex == 1)
                {
                    e.Graphics.DrawString(day.Month.ToString(), this.Font, Brushes.Black, e.Rect);
                }
                if (this.FixedRows <= e.RowIndex && e.ColIndex == 2)
                {
                    e.Graphics.DrawString(day.Day.ToString(), this.Font, Brushes.Black, e.Rect);
                }
            }
        }

        internal void Initialize(ViewData viewData)
        {
            this._viewData = viewData;
            var fixedRows = 2;
            var fixedCols = 3;
            this.Rows = _viewData.GetFilteredDays().Count + fixedRows;
            this.Cols = _viewData.GetFilteredMembers().Count + fixedCols;
            this.FixedRows = fixedRows;
            this.FixedCols = fixedCols;

            this.OnDrawCell += WorkItemGrid_OnDrawCell;
        }
    }
}
