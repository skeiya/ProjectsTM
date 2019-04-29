using System.Drawing;

namespace TaskManagement.Model
{
    public class MileStone
    {
        private string _name;
        private CallenderDay _day;
        private Color _color;

        public MileStone(string name, CallenderDay day, Color color)
        {
            this._name = name;
            this._day = day;
            this._color = color;
        }

        public string Name => _name;
        public CallenderDay Day => _day;
        public Color Color => _color;

        internal MileStone Clone()
        {
            return new MileStone(_name, _day, _color);
        }
    }
}
