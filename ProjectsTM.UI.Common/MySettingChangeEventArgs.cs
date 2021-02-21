using ProjectsTM.Model;

namespace ProjectsTM.UI.Common
{
    public class MySettingChangeEventArgs
    {
        private readonly Member _member;

        public string MemberName => _member == null ? string.Empty : _member.NaturalString;
        public MySettingChangeEventArgs(Member member)
        {
            _member = member;
        }
    }
}
