using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FreeGridControl
{
    [Serializable]
    public class IntArrayForDesign : System.Collections.ArrayList
    {

        public IntArrayForDesign()
        {
        }

        public event EventHandler ItemChanged;

        new public int this[int index]
        {
            get
            {
                return (int)base[index];
            }
            set
            {
                if (base[index].Equals(value)) return;
                base[index] = value;
                ItemChanged(this, null);
            }
        }

        public int[] ToIntArray()
        {
            var result = new List<int>();
            for(var idx = 0; idx < this.Count; idx++)
            {
                result.Add(this[idx]);
            }
            return result.ToArray();
        }

        internal int Sum(int r)
        {
            if (r == 0) return 0;
            var sum = 0;
            foreach (var i in this.GetRange(0, r))
            {
                sum += (int)i;
            }
            return sum;
        }

        public void SetCount(int newCount)
        {
            if (this.Count == newCount) return;
            if (this.Count < newCount)
            {
                var append = new List<int>();
                for (var index = 0; index < (newCount - this.Count); index++) append.Add(35);
                this.AddRange(append);
            }
            else
            {
                this.RemoveRange(newCount - 1, this.Count - newCount);
            }
            Debug.Assert(this.Count == newCount);
        }
    }
}