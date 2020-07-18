using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeGridControl
{
    public struct RawRectangle
    {
        private Rectangle _rectangle;

        public RawRectangle(int x, int y, int width, int height)
        {
            _rectangle = new Rectangle(x, y, width, height);
        }

        public int X => _rectangle.X;

        public bool IsEmpty => _rectangle.IsEmpty;

        public RectangleF Value => _rectangle;

        public int Width => _rectangle.Width;

        public int Height => _rectangle.Height;

        public int Y => _rectangle.Y;
    }
}
