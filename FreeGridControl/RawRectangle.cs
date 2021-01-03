using System;
using System.Collections.Generic;
using System.Drawing;

namespace FreeGridControl
{
    public struct RawRectangle : IEquatable<RawRectangle>
    {
        private Rectangle _rectangle;

        public RawRectangle(int x, int y, int width, int height)
        {
            _rectangle = new Rectangle(x, y, width, height);
        }

        public int X => _rectangle.X;

        public bool IsEmpty => _rectangle.IsEmpty;

        public Rectangle Value => _rectangle;

        public int Width => _rectangle.Width;

        public int Height => _rectangle.Height;

        public int Y => _rectangle.Y;

        public RawPoint Location => new RawPoint(X, Y);

        public override bool Equals(object obj)
        {
            return obj is RawRectangle rectangle && Equals(rectangle);
        }

        public bool Equals(RawRectangle other)
        {
            return EqualityComparer<Rectangle>.Default.Equals(_rectangle, other._rectangle);
        }

        public override int GetHashCode()
        {
            return 1131176703 + _rectangle.GetHashCode();
        }

        public static bool operator ==(RawRectangle left, RawRectangle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RawRectangle left, RawRectangle right)
        {
            return !(left == right);
        }
    }
}
