using ProjectsTM.Logic;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProjectsTM.Model
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

        public string Pattern = string.Empty;

        [XmlIgnore]
        public Color BackColor = Color.White;
        [XmlIgnore]
        public Color ForeColor = Color.Black;

        [XmlElement]
        public string ColorText
        {
            get { return ColorSerializer.Serialize(BackColor) + "/" + ColorSerializer.Serialize(ForeColor); }
            set
            {
                var m = Regex.Match(value, "(.+/.+/.+)/(.+/.+/.+)");
                BackColor = ColorSerializer.Deserialize(m.Groups[1].Value);
                ForeColor = ColorSerializer.Deserialize(m.Groups[2].Value);
            }
        }

        public override bool Equals(object obj)
        {
            var target = obj as ColorCondition;
            if (target == null) return false;
            if (!Pattern.Equals(target.Pattern)) return false;
            return BackColor.ToArgb() == target.BackColor.ToArgb();
        }

        internal XElement ToXml()
        {
            var xml = new XElement(nameof(ColorCondition));
            xml.Add(new XElement(nameof(Pattern), Pattern));
            xml.Add(new XElement(nameof(ColorText), ColorText));
            return xml;
        }

        internal static ColorCondition FromXml(XElement c)
        {
            var result = new ColorCondition();
            result.Pattern = c.Elements(nameof(Pattern)).Single().Value;
            result.ColorText = c.Elements(nameof(ColorText)).Single().Value;
            return result;
        }

        public override int GetHashCode()
        {
            var hashCode = 671444674;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Pattern);
            hashCode = hashCode * -1521134295 + EqualityComparer<Color>.Default.GetHashCode(BackColor);
            return hashCode;
        }

        public ColorCondition Clone()
        {
            return new ColorCondition(Pattern, BackColor, ForeColor);
        }

        public void Apply(ColorCondition cond)
        {
            this.Pattern = cond.Pattern;
            this.BackColor = cond.BackColor;
            this.ForeColor = cond.ForeColor;
        }
    }
}