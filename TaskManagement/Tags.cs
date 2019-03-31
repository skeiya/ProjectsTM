using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement
{
    public class Tags
    {
        private List<string> _tags = new List<string>();

        public Tags(List<string> result)
        {
            _tags = result;
        }

        public override string ToString()
        {
            if (_tags == null || _tags.Count == 0) return string.Empty;
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
    }
}
