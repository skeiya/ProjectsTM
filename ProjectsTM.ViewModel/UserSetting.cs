using ProjectsTM.Model;

namespace ProjectsTM.ViewModel
{
    public class UserSetting
    {
        public UserSetting() { }
        public string FilterName { get; set; }
        public float Ratio { get; set; }
        public int FontSize { get; set; }
        public string FilePath { get; set; }
        public Detail Detail { get; set; } = new Detail();
        public PatternHistory PatternHistory { get; set; } = new PatternHistory();
        public string UserName { get; set; } = "未設定";
        public bool HidePromptUserNameSetting { get; set; } = false;
    }
}
