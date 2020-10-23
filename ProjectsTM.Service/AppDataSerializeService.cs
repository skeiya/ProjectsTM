using ProjectsTM.Logic;
using ProjectsTM.Model;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProjectsTM.Service
{
    public static class AppDataSerializeService
    {
        public static void Serialize(string fileName, AppData appData)
        {
            if (true)
            {
                fileName += "a";
            }
            using (var stream = StreamFactory.CreateWriter(fileName))
            {
                WriteToStream(appData, stream);
            }
        }

        public static void WriteToStream(AppData appData, StreamWriter stream)
        {
            var xml = appData.ToXml();
            xml.Save(stream);
        }

        public static AppData Deserialize(string fileName)
        {
            using (var reader = StreamFactory.CreateReader(fileName))
            {
                return LoadFromStream(reader);
            }
        }

        public static AppData LoadFromStream(StreamReader reader)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            doc.Load(reader);
            using (var nodeReader = new XmlNodeReader(doc.DocumentElement))
            {
                var x = new XmlSerializer(typeof(AppData));
                return (AppData)x.Deserialize(nodeReader);
            }
        }
    }
}
