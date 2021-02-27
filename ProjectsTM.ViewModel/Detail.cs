using ProjectsTM.Model;
using System.Xml.Linq;

namespace ProjectsTM.ViewModel
{
    public class Detail
    {
        public int CompanyHeightCore { get; set; } = 10;
        public int NameHeightCore { get; set; } = 10;
        public int RowHeightCore { get; set; } = 10;
        public int DateWidthCore { get; set; } = 50;
        public int ColWidthCore { get; set; } = 20;
        public float ViewRatio { get; set; } = 1.0f;
        public Member Me = Member.Invalid;
        public bool HideSuggestionForUserNameSetting = false;
        internal Detail Clone()
        {
            var result = new Detail();
            result.CompanyHeightCore = this.CompanyHeightCore;
            result.NameHeightCore = this.NameHeightCore;
            result.RowHeightCore = this.RowHeightCore;
            result.DateWidthCore = this.DateWidthCore;
            result.ColWidthCore = this.ColWidthCore;
            result.ViewRatio = this.ViewRatio;
            return result;
        }

        internal XElement ToXml()
        {
            var xml = new XElement(nameof(Detail));
            xml.Add(new XElement(nameof(CompanyHeightCore)) { Value = CompanyHeightCore.ToString() });
            xml.Add(new XElement(nameof(NameHeightCore)) { Value = NameHeightCore.ToString() });
            xml.Add(new XElement(nameof(RowHeightCore)) { Value = RowHeightCore.ToString() });
            xml.Add(new XElement(nameof(DateWidthCore)) { Value = DateWidthCore.ToString() });
            xml.Add(new XElement(nameof(ColWidthCore)) { Value = ColWidthCore.ToString() });
            xml.Add(new XElement(nameof(ViewRatio)) { Value = ViewRatio.ToString() });
            xml.Add(Me.ToXml());
            xml.Add(new XElement("HideSuggestionForUserNameSetting") { Value = HideSuggestionForUserNameSetting.ToString() });
            return xml;
        }

        internal static Detail FromXml(XElement xml)
        {
            var result = new Detail();
            xml = xml.Element(nameof(Detail));
            result.CompanyHeightCore = int.Parse(xml.Element(nameof(CompanyHeightCore)).Value);
            result.NameHeightCore = int.Parse(xml.Element(nameof(NameHeightCore)).Value);
            result.RowHeightCore = int.Parse(xml.Element(nameof(RowHeightCore)).Value);
            result.DateWidthCore = int.Parse(xml.Element(nameof(DateWidthCore)).Value);
            result.ColWidthCore = int.Parse(xml.Element(nameof(ColWidthCore)).Value);
            result.ViewRatio = float.Parse(xml.Element(nameof(ViewRatio)).Value);
            if (xml.Element("Member") != null)
            {
                result.Me = Member.FromXml(xml.Element("Member"));
            }
            if (xml.Element("HideSuggestionForUserNameSetting") != null)
            {
                result.HideSuggestionForUserNameSetting = bool.Parse(xml.Element("HideSuggestionForUserNameSetting").Value);
            }
            return result;
        }
    }
}
