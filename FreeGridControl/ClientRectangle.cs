using System;
using System.Collections.Generic;
using System.Drawing;

namespace FreeGridControl
{
    public struct ClientRectangle : IEquatable<ClientRectangle>
    {
        private Rectangle _rectangle;

        public static ClientRectangle Empty => _empty;
        private static readonly ClientRectangle _empty = new ClientRectangle(Rectangle.Empty.X, Rectangle.Empty.Y, Rectangle.Empty.Width, Rectangle.Empty.Height);

        public ClientRectangle(int x, int y, int width, int height)
        {
            _rectangle = new Rectangle(x, y, width, height);
        }

        public bool Contains(ClientPoint location)
        {
            return _rectangle.Contains(location.X, location.Y);
        }

        public int Top => _rectangle.Top;
        public int X => _rectangle.X;
        public int Width => _rectangle.Width;
        public int Bottom => _rectangle.Bottom;

        public bool IsEmpty => _rectangle.IsEmpty;

        public Rectangle Value => _rectangle;

        public int Height => _rectangle.Height;

        public int Y
        {
            get { return _rectangle.Y; }
            set { _rectangle.Y = value; }
        }

        public int Left => _rectangle.Left;

        public void Intersect(ClientRectangle clientRectangle)
        {
            _rectangle.Intersect(clientRectangle.Value);
        }

        public void Offset(int x, int y)
        {
            _rectangle.Offset(x, y);
        }

        internal bool IntersectsWith(ClientRectangle visible)
        {
            return _rectangle.IntersectsWith(visible.Value);
        }

        public bool Contains(ClientRectangle value)
        {
            return _rectangle.Contains(value.Value);
        }

        public void Inflate(int x, int y)
        {
            _rectangle.Inflate(x, y);
        }

        public override bool Equals(object obj)
        {
            return obj is ClientRectangle rectangle && Equals(rectangle);
        }

        public bool Equals(ClientRectangle other)
        {
            return EqualityComparer<Rectangle>.Default.Equals(_rectangle, other._rectangle);
        }

        public override int GetHashCode()
        {
            return 1131176703 + _rectangle.GetHashCode();
        }

        public static bool operator ==(ClientRectangle left, ClientRectangle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ClientRectangle left, ClientRectangle right)
        {
            return !(left == right);
        }
    }
}
