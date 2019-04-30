using System.IO;
using System.Xml.Serialization;
using TaskManagement.Model;

namespace TaskManagement.Service
{
    class UserSettingUIService
    {
        internal static void Save(string filePath, UserSetting setting)
        {
            var xml = new XmlSerializer(typeof(UserSetting));
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            using (var w = new StreamWriter(filePath)) {
                xml.Serialize(w, setting);
            }
        }

        internal static UserSetting Load(string filePath)
        {
            var xml = new XmlSerializer(typeof(UserSetting));
            using(var r = new StreamReader(filePath))
            {
                return (UserSetting)xml.Deserialize(r);
            }
        }

    }
}
