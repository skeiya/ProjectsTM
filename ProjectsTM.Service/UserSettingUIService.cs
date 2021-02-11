using ProjectsTM.ViewModel;
using System;
using System.IO;
using System.Xml.Linq;

namespace ProjectsTM.Service
{
    public static class UserSettingUIService
    {
        private static string AppConfigDir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ProjectsTM");
        private static string UserSettingPath => Path.Combine(AppConfigDir, "UserSetting.xml");
        public static void Save(UserSetting setting)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(UserSettingPath));
            var xml = setting.ToXml();
            xml.Save(UserSettingPath);
        }

        public static UserSetting Load()
        {
            var xml = XElement.Load(UserSettingPath);
            return UserSetting.FromXml(xml);
        }

    }
}
