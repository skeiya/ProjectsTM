using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace TaskManagement
{
    public class ColorCondition
    {
        public ColorCondition() { }

        public ColorCondition(string v, Color c)
        {
            this.Pattern = v;
            this.Color = c;
        }

        public string Pattern;

        [XmlIgnore]
        public Color Color;

        [XmlElement]
        public string ColorText
        {
            get { return Color.R.ToString() + "/" + Color.G.ToString() + "/" + Color.B.ToString(); }
            set
            {
                var words = value.Split('/');
                var r = int.Parse(words[0]);
                var g = int.Parse(words[1]);
                var b = int.Parse(words[2]);
                Color = Color.FromArgb(r, g, b);
            }
        }

        public override bool Equals(object obj)
        {
            var target = obj as ColorCondition;
            if (target == null) return false;
            if (!Pattern.Equals(target.Pattern)) return false;
            return Color.ToArgb() == target.Color.ToArgb();
        }

        public override int GetHashCode()
        {
            var hashCode = 671444674;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Pattern);
            hashCode = hashCode * -1521134295 + EqualityComparer<Color>.Default.GetHashCode(Color);
            return hashCode;
        }
    }
}