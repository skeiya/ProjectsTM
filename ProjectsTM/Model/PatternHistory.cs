using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace ProjectsTM.Model
{
    public class PatternHistory
    {
        [XmlIgnore]
        public ReadOnlyCollection<string> Items => ListCore.Reverse<string>().ToList().AsReadOnly();

        [XmlElement]
        public List<string> ListCore { get; } = new List<string>();

        private static int Depth => 20;

        public PatternHistory()
        {
        }

        internal void Append(string text)
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
    }
}