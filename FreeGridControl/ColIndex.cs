using System.Collections.Generic;
using System.Diagnostics;

namespace FreeGridControl
{
    [DebuggerDisplay("Index = {Value}")]
    public class ColIndex
    {
        public ColIndex(int value)
        {
            this.Value = value;
        }

        public int Value { get; internal set; }

        public static IEnumerable<ColIndex> Range(int start, int count)
        {
            for (var i = start; i < start + count; i++)
            {
                yield return new ColIndex(i);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is ColIndex index &&
                   Value == index.Value;
        }

        public override int GetHashCode()
        {
            return -1937169414 + Value.GetHashCode();
        }

        public ColIndex Offset(int offset)
        {
            return new ColIndex(this.Value + offset);
        }

        public IEnumerable<ColIndex> Range(int count)
        {
            for (var i = this.Value; i < this.Value + count; i++)
            {
                yield return new ColIndex(i);
            }
        }
    }
}
