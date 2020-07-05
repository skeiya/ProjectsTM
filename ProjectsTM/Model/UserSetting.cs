using ProjectsTM.ViewModel;

namespace ProjectsTM.Model
{
    public class UserSetting
    {
        public UserSetting() { }
        public string FilterName { set; get; }
        public float Ratio { set; get; }
        public int FontSize { set; get; }
        public string FilePath { get; set; }
        public Detail Detail { set; get; } = new Detail();
    }
}
