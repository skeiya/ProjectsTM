using ProjectsTM.Logic;
using ProjectsTM.Model;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace ProjectsTM.Service
{
    public static class AppDataSerializeService
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
            var xml = appData.ToXml();
            xml.Save(stream);
        }

        public static AppData Deserialize(string fileName)
        {
            using (var reader = StreamFactory.CreateReader(fileName))
            {
                return AppData.FromXml(XElement.Load(fileName, LoadOptions.SetLineInfo));
            }
        }

        public static AppData LoadFromStream(StreamReader reader)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            using (var nodeReader = new XmlNodeReader(doc.DocumentElement))
            {
                return AppData.FromXml(XElement.Load(nodeReader));
            }
        }

        public static AppData LoadFromString(string str)
        {
            using (var reader = StreamFactory.CreateReaderFromString(str))
            {
                return LoadFromStream(reader);
            }
        }
    }
}
