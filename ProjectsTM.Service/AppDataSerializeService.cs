using ProjectsTM.Logic;
using ProjectsTM.Model;
using System.IO;
using System.Linq;
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

        public static AppData LoadFromStream(object reader, bool isOld)
        {
            var doc = GetXmlDoc(reader);
            if (doc == null) return null;
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
            return LoadFromStream(xml, IsOldFomatFileContent(new StringReader(xml)));
        }

        private static XmlDocument GetXmlDoc(object reader)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            if (typeof(StreamReader) == reader.GetType()) { doc.Load((StreamReader)reader); return doc; }
            if (typeof(string) == reader.GetType()) { doc.LoadXml((string)reader); return doc; }
            return null;
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
