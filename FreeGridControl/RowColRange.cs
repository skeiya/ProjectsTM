using System.Collections.Generic;

namespace FreeGridControl
{
    public class RowColRange
    {
        private ColIndex _leftCol;
        private RowIndex _topRow;
        private int _colCount;
        private int _rowCount;

        public RowColRange(ColIndex visibleNormalLeftCol, RowIndex visibleNormalTopRow, int visibleNormalColCount, int visibleNormalRowCount)
        {
            this._leftCol = visibleNormalLeftCol;
            this._topRow = visibleNormalTopRow;
            this._colCount = visibleNormalColCount;
            this._rowCount = visibleNormalRowCount;
        }

        public IEnumerable<ColIndex> Cols
        {
            get
            {
                return _leftCol.Range(_colCount);
            }
        }



        public IEnumerable<RowIndex> Rows
        {
            get
            {
                return _topRow.Range(_rowCount);
            }
        }

        public int RowCount => _rowCount;
        public RowIndex TopRow => _topRow;

        public ColIndex LeftCol => _leftCol;
    }
}