using ProjectsTM.Model;

namespace ProjectsTM.UI.Common
{
    public class MySettingChageEventArgs
    {
        private readonly Member _member;

        public string MemberName => _member == null ? string.Empty : _member.NaturalString;
        public MySettingChageEventArgs(Member member)
        {
            _member = member;
        }
    }
}
