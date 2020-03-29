using System;
using System.Drawing;

namespace FreeGridControl
{
    public class DrawNormalAreaEventArgs
    {
        public DrawNormalAreaEventArgs(Graphics graphics)
        {
            Graphics = graphics;
        }
        public Graphics Graphics { get; set; }
    }
}