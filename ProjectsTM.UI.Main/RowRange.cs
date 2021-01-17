using FreeGridControl;

namespace ProjectsTM.UI.Main
{
    class RowRange
    {
        public RowIndex Row { get; private set; }
        public int Count { get; private set; }

        public RowRange(RowIndex row, int count)
        {
            this.Row = row;
            this.Count = count;
        }
    }
}
