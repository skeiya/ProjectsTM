using System.Collections.Generic;

namespace FreeGridControl
{
    public class RowIndex
    {
        public RowIndex(int value)
        {
            this.Value = value;
        }

        public int Value { get; internal set; }

        public static IEnumerable<RowIndex> Range(int start, int count)
        {
            for (var i = start; i < start + count; i++)
            {
                yield return new RowIndex(i);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is RowIndex index &&
                   Value == index.Value;
        }

        public override int GetHashCode()
        {
            return -1937169414 + Value.GetHashCode();
        }

        public RowIndex Offset(int offset)
        {
            return new RowIndex(this.Value + offset);
        }

        public IEnumerable<RowIndex> Range(int count)
        {
            for (var i = this.Value; i < this.Value + count; i++)
            {
                yield return new RowIndex(i);
            }
        }
    }
}
