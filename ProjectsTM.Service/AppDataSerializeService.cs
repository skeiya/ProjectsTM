using ProjectsTM.Logic;
using ProjectsTM.Model;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

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
                return LoadFromStream(reader, IsOldFormat(fileName));
            }
        }

        public static AppData LoadFromStream(StreamReader reader, bool isOld)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            doc.Load(reader);
            using (var nodeReader = new XmlNodeReader(doc.DocumentElement))
            {
                if (isOld)
                {
                    var x = new XmlSerializer(typeof(AppData));
                    return (AppData)x.Deserialize(nodeReader);
                }
                else
                {
                    return AppData.FromXml(XElement.Load(nodeReader));
                }
            }
        }

        public static AppData DeserializeFileContent(string xml)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(xml);
            MemoryStream stream = new MemoryStream(byteArray);
            return LoadFromStream(new StreamReader(stream), IsOldFomatFileContent(new StringReader(xml)));
        }

        private static bool IsOldFormat(string path)
        {
            using (var reader = StreamFactory.CreateReader(path))
            {
                return IsOldFomatFileContent(reader);
            }
        }

        private static bool IsOldFomatFileContent(TextReader reader)
        {
            var xml = XElement.Load(reader);
            if (!xml.Elements("Version").Any()) return true;
            var ver = int.Parse(xml.Elements("Version").Single().Value);
            return ver < 5;
        }
    }
}
