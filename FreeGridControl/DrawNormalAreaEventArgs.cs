using System.Drawing;

namespace FreeGridControl
{
    public class DrawNormalAreaEventArgs
    {
        public DrawNormalAreaEventArgs(Graphics graphics, bool isAllDraw)
        {
            Graphics = graphics;
            IsAllDraw = isAllDraw;
        }
        public Graphics Graphics { get; set; }
        public bool IsAllDraw { get; }
    }
}