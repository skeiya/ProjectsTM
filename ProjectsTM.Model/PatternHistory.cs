using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ProjectsTM.Model
{
    public class PatternHistory
    {
        public ReadOnlyCollection<string> Items => ListCore.Reverse<string>().ToList().AsReadOnly();

        private List<string> ListCore { get; } = new List<string>();

        private static int Depth => 20;

        public PatternHistory()
        {
        }

        public void Append(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            if (IsNewestSame(text)) return;
            if (ListCore.Contains(text))
            {
                ListCore.Remove(text);

            }
            ListCore.Add(text);
            if (Depth <= ListCore.Count)
            {
                ListCore.RemoveRange(0, ListCore.Count - Depth);
            }
        }

        private bool IsNewestSame(string text)
        {
            if (ListCore.Count == 0) return false;
            return ListCore[ListCore.Count - 1].Equals(text);
        }

        public XElement ToXml()
        {
            var xml = new XElement(nameof(PatternHistory));
            ListCore.ForEach(h => xml.Add(new XElement("Pattern") { Value = h.ToString() }));
            return xml;
        }

        public static PatternHistory FromXml(XElement xml)
        {
            var result = new PatternHistory();
            if (xml.Element(nameof(PatternHistory)) == null) return result;
            foreach (var p in xml.Element(nameof(PatternHistory)).Elements("Pattern"))
            {
                result.ListCore.Add(p.Value);
            }
            return result;
        }

        public void Load(string path)
        {
            if (!File.Exists(path)) return;
            var xml = XElement.Load(path);
            var h = PatternHistory.FromXml(xml);
            foreach (var p in h.Items)
            {
                Append(p);
            }
        }

        public void CopyFrom(PatternHistory patternHistory)
        {
            this.ListCore.AddRange(patternHistory.Items);
        }
    }
}