using System;
using System.Drawing;
using System.Windows.Forms;

namespace FreeGridControl
{
    public struct ClientPoint
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
    }
}
