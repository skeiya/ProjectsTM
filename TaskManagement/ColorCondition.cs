using System.Drawing;
using System.Runtime.Serialization;

namespace TaskManagement
{
    public class ColorCondition
    {
        private string _regex;
        private Color _color;

        public ColorCondition(string v, Color c)
        {
            this._regex = v;
            this._color = c;
        }

        public string Regex => _regex;
        public Color Color => _color;
    }
}