using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace TaskManagement
{
    internal class AppDataSerializer
    {
        private const string DAY_TAG = "<@DAY_TAG>";
        private const string MEMBER_TAG = "<@MEMBER_TAG>";
        private const string WORKITEM_TAG = "<@WORKITEM_TAG>";

        internal static void Serialize(string fileName, AppData appData)
        {
            using (var stream = new StreamWriter(fileName))
            {
                foreach (var d in appData.Callender.Days)
                {
                    stream.WriteLine(DAY_TAG + d.ToString());
                }
                foreach (var m in appData.Members)
                {
                    stream.WriteLine(MEMBER_TAG + m.ToSerializeString());
                }
                foreach (var t in appData.WorkItems)
                {
                    stream.WriteLine(WORKITEM_TAG + t.ToSerializeString());
                }
            }
        }

        internal static AppData Deserialize(string fileName)
        {
            var result = new AppData();
            using (var reader = new StreamReader(fileName))
            {
                while (true)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line)) return result;

                    var m = Regex.Match(line, DAY_TAG + "(.*)");
                    if (m.Success)
                    {
                        result.Callender.Add(CallenderDay.Parse(m.Groups[1].Value));
                        continue;
                    }
                    m = Regex.Match(line, MEMBER_TAG + "(.*)");
                    if (m.Success)
                    {
                        result.Members.Add(Member.Parse(m.Groups[1].Value));
                        continue;
                    }
                    m = Regex.Match(line, WORKITEM_TAG + "(.*)");
                    if (m.Success)
                    {
                        result.WorkItems.Add(WorkItem.Parse(m.Groups[1].Value, result.Callender));
                        continue;
                    }
                }
            }
        }
    }
}
