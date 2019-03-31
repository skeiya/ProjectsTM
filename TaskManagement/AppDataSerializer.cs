using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace TaskManagement
{
    public class AppDataSerializer
    {
        private const string DAY_TAG = "<@DAY_TAG>";
        private const string MEMBER_TAG = "<@MEMBER_TAG>";
        private const string WORKITEM_TAG = "<@WORKITEM_TAG>";

        public static void Serialize(string fileName, AppData appData)
        {
            using (var stream = new StreamWriter(fileName))
            {
                WriteToStream(appData, stream);
            }
        }

        public static void WriteToStream(AppData appData, StreamWriter stream)
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

        public static AppData Deserialize(string fileName, out string error)
        {
            using (var reader = new StreamReader(fileName))
            {
                return LoadFromStream(out error, reader);
            }
        }

        public static AppData LoadFromStream(out string error, StreamReader reader)
        {
            var result = new AppData();

            while (true)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    error = string.Empty;
                    return result;
                }

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
                    var w = WorkItem.Parse(m.Groups[1].Value, result.Callender);
                    if (!result.Callender.Days.Contains(w.Period.From) ||
                        !result.Callender.Days.Contains(w.Period.To))
                    {
                        error = "day error" + w.Period.From + "/" + w.Period.To;
                        return null;
                    }
                    if (!result.Members.Contain(w.AssignedMember))
                    {
                        error = "member error" + w.AssignedMember.ToSerializeString();
                        return null;
                    }
                    result.WorkItems.Add(w);
                    continue;
                }
            }
        }
    }
}
