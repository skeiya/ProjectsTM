using System.IO;
using System.Xml;
using System.Xml.Serialization;
using TaskManagement.Model;

namespace TaskManagement.Logic
{
    public static class AppDataSerializer
    {
        public static void Serialize(string fileName, AppData appData)
        {
            using (var stream = StreamFactory.CreateWriter(fileName))
            {
                WriteToStream(appData, stream);
            }
        }

        public static void WriteToStream(AppData appData, StreamWriter stream)
        {
            var serializer = new XmlSerializer(typeof(AppData));
            serializer.Serialize(stream, appData);
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
