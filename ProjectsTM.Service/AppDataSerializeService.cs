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

        public static AppData LoadFromStream(StreamReader reader, bool isOld)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            using (var nodeReader = new XmlNodeReader(doc.DocumentElement))
            {
                if (isOld)
                {
                    var x = new XmlSerializer(typeof(AppData));
                    var tmp = (AppData)x.Deserialize(nodeReader);
                    using (var tmpReader = new XmlNodeReader(doc.DocumentElement))
                    {
                        foreach (var callenderDay in XElement.Load(tmpReader).Element("Callender").Element("Days").Elements("CallenderDay"))
                        {
                            var ca = CallenderDay.Parse(callenderDay.Element("Date").Value);
                            tmp.Callender.Add(ca);
                        }
                    }
                    return tmp;
                }
                else
                {
                    return AppData.FromXml(XElement.Load(nodeReader));
                }
            }
        }

        public static AppData LoadFromString(string str)
        {
            using (var reader = StreamFactory.CreateReaderFromString(str))
            {
                return LoadFromStream(reader, IsOldFormat(new StringReader(str)));
            }
        }

        private static bool IsOldFormat(string path)
        {
            using (var reader = StreamFactory.CreateReader(path))
            {
                return IsOldFormat(reader);
            }
        }

        private static bool IsOldFormat(TextReader reader)
        {
            var xml = XElement.Load(reader);
            if (!xml.Elements("Version").Any()) return true;
            var ver = int.Parse(xml.Elements("Version").Single().Value);
            return ver < 5;
        }
    }
}
