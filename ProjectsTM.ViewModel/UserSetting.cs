using ProjectsTM.Model;
using System.Xml.Linq;

namespace ProjectsTM.ViewModel
{
    public class UserSetting
    {
        public UserSetting() { }
        public string FilterName { get; set; }
        public float Ratio { get; set; }
        public int FontSize { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public Detail Detail { get; set; } = new Detail();
        public PatternHistory PatternHistory { get; set; } = new PatternHistory();
        public string UserName { get; set; } = "未設定";
        public bool HideSuggestionForUserNameSetting { get; set; } = false;

        public XElement ToXml()
        {
            var xml = new XElement(nameof(UserSetting));
            xml.Add(new XElement(nameof(FilterName)) { Value = FilterName.ToString() });
            xml.Add(new XElement(nameof(Ratio)) { Value = Ratio.ToString() });
            xml.Add(new XElement(nameof(FontSize)) { Value = FontSize.ToString() });
            xml.Add(new XElement(nameof(FilePath)) { Value = FilePath.ToString() });
            xml.Add(Detail.ToXml());
            xml.Add(PatternHistory.ToXml());
            xml.Add(new XElement(nameof(UserName)) { Value = UserName.ToString() });
            xml.Add(new XElement(nameof(HideSuggestionForUserNameSetting)) { Value = HideSuggestionForUserNameSetting.ToString() });
            return xml;
        }

        public static UserSetting FromXml(XElement xml)
        {
            var result = new UserSetting();
            result.FilterName = xml.Element(nameof(FilterName)).Value;
            result.Ratio = float.Parse(xml.Element(nameof(Ratio)).Value);
            result.FontSize = int.Parse(xml.Element(nameof(FontSize)).Value);
            if (xml.Element(nameof(FilePath)) != null)
            {
                result.FilePath = xml.Element(nameof(FilePath)).Value;
            }
            result.Detail = Detail.FromXml(xml);
            result.PatternHistory = PatternHistory.FromXml(xml);
            if (xml.Element(nameof(UserName)) != null)
            {
                result.UserName = xml.Element(nameof(UserName)).Value;
            }
            if (xml.Element(nameof(HideSuggestionForUserNameSetting)) != null)
            {
                result.HideSuggestionForUserNameSetting = bool.Parse(xml.Element(nameof(HideSuggestionForUserNameSetting)).Value);
            }
            return result;
        }
    }
}
