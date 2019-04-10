using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace TaskManagement
{
    public class AppDataSerializer
    {
        public static void Serialize(string fileName, AppData appData)
        {
            using (var stream = new StreamWriter(fileName))
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
            using (var reader = new StreamReader(fileName))
            {
                return LoadFromStream(reader);
            }
        }

        public static AppData LoadFromStream(StreamReader reader)
        {
            var serializer = new XmlSerializer(typeof(AppData));
            return (AppData)serializer.Deserialize(reader);
        }
    }
}
