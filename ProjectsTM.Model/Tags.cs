﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ProjectsTM.Model
{
    public class Tags
    {
        private readonly List<string> _tags = new List<string>();

        public Tags(IEnumerable<string> result)
        {
            _tags = result.ToList();
        }

        public override string ToString()
        {
            if (_tags.Count == 0) return string.Empty;
            var result = _tags[0];
            for (int index = 1; index < _tags.Count; index++)
            {
                result += "|" + _tags[index];
            }
            return result;
        }

        public static Tags Parse(string text)
        {
            return new Tags(text.Split('|').ToList());
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Tags target)) return false;
            return _tags.SequenceEqual(target._tags);
        }

        public override int GetHashCode()
        {
            return -411299617 + EqualityComparer<List<string>>.Default.GetHashCode(_tags);
        }

        internal string ToDrawString()
        {
            if (_tags.Count == 0) return string.Empty;
            var result = new StringBuilder();
            foreach (var t in _tags)
            {
                result.Append(t + Environment.NewLine);
            }
            result.Remove(result.Length - 1, 1);
            return result.ToString();
        }

        internal XElement ToXml()
        {
            var xml = new XElement(nameof(Tags))
            {
                Value = ToString(),
            };
            return xml;
        }

        internal static Tags FromXml(XElement w)
        {
            return Parse(w.Element(nameof(Tags)).Value);
        }
    }
}
