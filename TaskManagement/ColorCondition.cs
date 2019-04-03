using System;
using System.Collections.Generic;
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

        public override bool Equals(object obj)
        {
            return obj is ColorCondition condition &&
                   _regex == condition._regex &&
                   EqualityComparer<Color>.Default.Equals(_color, condition._color);
        }

        public override int GetHashCode()
        {
            var hashCode = 671444674;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_regex);
            hashCode = hashCode * -1521134295 + EqualityComparer<Color>.Default.GetHashCode(_color);
            return hashCode;
        }

        internal string ToSerializeString()
        {
            return this._color.Name + "/" + this.Regex;
        }

        internal static ColorCondition Parse(string value)
        {
            var index = value.IndexOf('/');
            var color = Color.FromName((value.Substring(0, index)));
            var regex = value.Substring(index + 1, value.Length - index - 1);
            return new ColorCondition(regex, color);
        }
    }
}