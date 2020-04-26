using System.Drawing;

namespace FreeGridControl
{
    public class DrawNormalAreaEventArgs
    {
        public DrawNormalAreaEventArgs(Graphics graphics, bool isPrint)
        {
            Graphics = graphics;
            IsPrint = isPrint;
        }
        public Graphics Graphics { get; set; }
        public bool IsPrint { get; }
    }
}