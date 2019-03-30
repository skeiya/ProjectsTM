using System;
using System.IO;
using System.Xml.Serialization;

namespace TaskManagement
{
    internal class AppDataSerializer
    {
        internal static void Serialize(string fileName, AppData appData)
        {
            using (var stream = new StreamWriter(fileName))
            {
                foreach (var d in appData.Callender.Days)
                {
                    stream.WriteLine("<@DAY_TAG>" + d.ToString());
                }
                foreach (var m in appData.Members)
                {
                    stream.WriteLine("<@MEMBER_TAG>" + m.ToSerializeString());
                }
                foreach (var t in appData.WorkItems)
                {
                    stream.WriteLine("<@WORKITEM_TAG>" + t.ToString());
                }
            }
        }
    }
}
