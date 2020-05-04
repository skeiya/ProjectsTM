﻿using System.IO;
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
            using (var xmlReader = XmlReader.Create(reader))
            {
                return LoadFromStream(xmlReader);
            }
        }

        public static AppData LoadFromStream(XmlReader reader)
        {
            var serializer = new XmlSerializer(typeof(AppData));
            return (AppData)serializer.Deserialize(reader);
        }
    }
}
