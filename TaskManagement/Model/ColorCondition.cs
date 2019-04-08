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

        public ColorCondition(string v, Color back, Color fore)
        {
            this.Pattern = v;
            this.BackColor = back;
            this.ForeColor = fore;
        }

        public string Pattern;

        [XmlIgnore]
        public Color BackColor = Color.White;
        [XmlIgnore]
        public Color ForeColor = Color.Black;

        [XmlElement]
        public string ColorText
        {
            get { return BackColor.R.ToString() + "/" + BackColor.G.ToString() + "/" + BackColor.B.ToString() + "/"+ ForeColor.R.ToString() + "/" + ForeColor.G.ToString() + "/" + ForeColor.B.ToString(); }
            set
            {
                var words = value.Split('/');
                var r = int.Parse(words[0]);
                var g = int.Parse(words[1]);
                var b = int.Parse(words[2]);
                BackColor = Color.FromArgb(r, g, b);

                if (words.Length <= 3) return;
                var rf = int.Parse(words[3]);
                var gf = int.Parse(words[4]);
                var bf = int.Parse(words[5]);
                ForeColor = Color.FromArgb(rf, gf, bf);
            }
        }

        public override bool Equals(object obj)
        {
            var target = obj as ColorCondition;
            if (target == null) return false;
            if (!Pattern.Equals(target.Pattern)) return false;
            return BackColor.ToArgb() == target.BackColor.ToArgb();
        }

        public override int GetHashCode()
        {
            var hashCode = 671444674;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Pattern);
            hashCode = hashCode * -1521134295 + EqualityComparer<Color>.Default.GetHashCode(BackColor);
            return hashCode;
        }
    }
}