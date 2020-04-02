using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FreeGridControl
{
    [Serializable]
    public class FloatArrayForDesign : System.Collections.ArrayList
    {

        public FloatArrayForDesign()
        {
        }

        public event EventHandler ItemChanged;

        new public float this[int index]
        {
            get
            {
                return (float)base[index];
            }
            set
            {
                if (base[index].Equals(value)) return;
                base[index] = value;
                ItemChanged(this, null);
            }
        }

        internal float Sum(int r)
        {
            if (r == 0) return 0;
            var sum = 0f;
            foreach (var i in this.GetRange(0, r))
            {
                sum += (float)i;
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