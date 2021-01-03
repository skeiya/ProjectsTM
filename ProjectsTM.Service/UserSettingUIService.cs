using ProjectsTM.Logic;
using ProjectsTM.ViewModel;
using System;
using System.IO;
using System.Xml.Serialization;

namespace ProjectsTM.Service
{
    public static class UserSettingUIService
    {
        private static string AppConfigDir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ProjectsTM");
        private static string UserSettingPath => Path.Combine(AppConfigDir, "UserSetting.xml");
        public static void Save(UserSetting setting)
        {
            var xml = new XmlSerializer(typeof(UserSetting));
            Directory.CreateDirectory(Path.GetDirectoryName(UserSettingPath));
            using (var w = StreamFactory.CreateWriter(UserSettingPath))
            {
                xml.Serialize(w, setting);
            }
        }

        public static UserSetting Load()
        {
            var xml = new XmlSerializer(typeof(UserSetting));
            using (var r = StreamFactory.CreateReader(UserSettingPath))
            {
                return (UserSetting)xml.Deserialize(r);
            }
        }

    }
}
