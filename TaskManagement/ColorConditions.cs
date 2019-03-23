using System;
using System.Collections;
using System.Collections.Generic;

namespace TaskManagement
{
    public class ColorConditions : IEnumerable<ColorCondition>
    {
        private List<ColorCondition> _list = new List<ColorCondition>();

        public IEnumerator<ColorCondition> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        internal void Add(ColorCondition cond)
        {
            _list.Add(cond);
        }
    }
}