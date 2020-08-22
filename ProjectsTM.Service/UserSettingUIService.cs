using ProjectsTM.Logic;
using ProjectsTM.ViewModel;
using System.IO;
using System.Xml.Serialization;

namespace ProjectsTM.Service
{
    public class UserSettingUIService
    {
        public static void Save(string filePath, UserSetting setting)
        {
            var xml = new XmlSerializer(typeof(UserSetting));
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            using (var w = StreamFactory.CreateWriter(filePath))
            {
                xml.Serialize(w, setting);
            }
        }

        public static UserSetting Load(string filePath)
        {
            var xml = new XmlSerializer(typeof(UserSetting));
            using (var r = StreamFactory.CreateReader(filePath))
            {
                return (UserSetting)xml.Deserialize(r);
            }
        }

    }
}
