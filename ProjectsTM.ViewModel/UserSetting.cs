using ProjectsTM.Model;

namespace ProjectsTM.ViewModel
{
    public class UserSetting
    {
        public UserSetting() { }
        public string FilterName { set; get; }
        public float Ratio { set; get; }
        public int FontSize { set; get; }
        public string FilePath { get; set; }
        public Detail Detail { set; get; } = new Detail();
        public PatternHistory PatternHistory { set; get; } = new PatternHistory();
        public string UserName { set; get; } = "未設定";
    }
}
