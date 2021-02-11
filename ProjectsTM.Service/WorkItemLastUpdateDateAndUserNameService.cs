using ProjectsTM.Model;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace ProjectsTM.Service
{
    public class WorkItemLastUpdateDateAndUserNameService
    {
        private string _filePath;
        private Dictionary<WorkItem, Tuple<int, int>> _xmlLines;

        public void Load(string filePath)
        {
            _filePath = filePath;
            var xmlWorkItems = GetXmlLines(XElement.Load(filePath, LoadOptions.SetLineInfo).Element(nameof(WorkItems)));
        }

        private Dictionary<WorkItem, Tuple<int, int>> GetXmlLines(XElement e)
        {
            _xmlLines = new Dictionary<WorkItem, Tuple<int, int>>();
            foreach (var m in e.Elements("WorkItemsOfEachMember"))
            {
                var assign = Member.Parse(m.Attribute("Name").Value);
                foreach (var w in m.Element(nameof(MembersWorkItems))
                    .Elements(nameof(WorkItem)))
                {
                    var lines = GetStartLineAndEndLine(w);
                    if (lines.Item1 == 0 || lines.Item2 == 0) continue;
                    _xmlLines.Add(WorkItem.FromXml(w, assign), new Tuple<int,int>(lines.Item1, lines.Item2));
                }
            }
            return _xmlLines;
        }

        private static Tuple<int, int> GetStartLineAndEndLine(XElement workItemElement)
        {
            int startLine = 0; int endline = 0;
            var reader = workItemElement.CreateReader();
            while (reader.Read())
            {
                if (reader.Name != "WorkItem") continue;
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        startLine = ((IXmlLineInfo)reader).LineNumber;
                        break;
                    case XmlNodeType.EndElement:
                        endline = ((IXmlLineInfo)reader).LineNumber;
                        break;
                    default:
                        break;
                }
            }
            return new Tuple<int, int>(startLine, endline);
        }

        public string GetDateAndUserName(WorkItem workItem)
        {
            if (_xmlLines == null) return string.Empty;
            Tuple<int, int> pair;
            WorkItem key = new WorkItem();
            foreach(var k in _xmlLines.Keys)
            {
                if (k.Equals(workItem)) { key = k; break; }
            }
            if (!_xmlLines.TryGetValue(key, out pair)) return string.Empty;
            var result = GitRepositoryService.GetLastUpdateDateAndUserName(_filePath, pair.Item1, pair.Item2);
            if (!string.IsNullOrEmpty(result)) return result;
            return string.Empty;
        }
    }
}
