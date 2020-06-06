using System.Windows.Forms;

namespace FreeGridControl
{
    public partial class GridControl : UserControl
    {
        public struct RawPoint
        {
            public int X { get; }
            public int Y { get; }
            public RawPoint(int x = 0, int y = 0) { X = x; Y = y; }
        }
    }
}
