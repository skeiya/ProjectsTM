using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectsTM.UI
{
    public class PatternHistory
    {
        public string[] Items => _list.Reverse<string>().ToArray();

        public event EventHandler Updated;

        private List<string> _list = new List<string>();

        private static int Depth => 20;

        public PatternHistory()
        {
        }

        internal void Append(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            if (IsNewestSame(text)) return;
            if (_list.Contains(text))
            {
                _list.Remove(text);

            }
            _list.Add(text);
            if (Depth <= _list.Count)
            {
                _list.RemoveRange(0, _list.Count - Depth);
            }
            Updated?.Invoke(this, null);
        }

        private bool IsNewestSame(string text)
        {
            if (_list.Count == 0) return false;
            return _list[_list.Count - 1].Equals(text);
        }
    }
}