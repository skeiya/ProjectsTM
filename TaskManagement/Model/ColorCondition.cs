using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace TaskManagement
{
    public class ColorCondition
    {
        private string _pattern;
        private Color _color;

        public ColorCondition(string v, Color c)
        {
            this._pattern = v;
            this._color = c;
        }

        public string Pattern => _pattern;
        public Color Color => _color;

        public override bool Equals(object obj)
        {
            var target = obj as ColorCondition;
            if (target == null) return false;
            if (!_pattern.Equals(target._pattern)) return false;
            return _color.ToArgb() == target._color.ToArgb();
        }

        public override int GetHashCode()
        {
            var hashCode = 671444674;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_pattern);
            hashCode = hashCode * -1521134295 + EqualityComparer<Color>.Default.GetHashCode(_color);
            return hashCode;
        }

        internal string ToSerializeString()
        {
            return _color.R.ToString() + "/" + _color.G.ToString() + "/" + _color.B.ToString() + "/" + this.Pattern;
        }

        internal static ColorCondition Parse(string value)
        {
            var m = Regex.Match(value, "(.+)/(.*)/(.*)/(..*$)");
            var r = int.Parse(m.Groups[1].Value);
            var g = int.Parse(m.Groups[2].Value);
            var b = int.Parse(m.Groups[3].Value);
            var color = Color.FromArgb(r, g, b);
            var regex = m.Groups[4].Value;
            return new ColorCondition(regex, color);
        }
    }
}