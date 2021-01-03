using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FreeGridControl
{
    public struct ClientPoint : IEquatable<ClientPoint>
    {
        private Point _location;

        public ClientPoint(Point location)
        {
            this._location = location;
        }

        public ClientPoint(int v1, int v2)
        {
            _location = new Point(v1, v2);
        }

        public int X => _location.X;
        public int Y => _location.Y;

        public static ClientPoint Create(MouseEventArgs e)
        {
            return new ClientPoint(e.Location);
        }

        public override bool Equals(object obj)
        {
            return obj is ClientPoint point && Equals(point);
        }

        public bool Equals(ClientPoint other)
        {
            return EqualityComparer<Point>.Default.Equals(_location, other._location);
        }

        public override int GetHashCode()
        {
            return 1850136813 + _location.GetHashCode();
        }

        public static bool operator ==(ClientPoint left, ClientPoint right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ClientPoint left, ClientPoint right)
        {
            return !(left == right);
        }
    }
}
