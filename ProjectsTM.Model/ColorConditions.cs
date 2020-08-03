﻿using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ProjectsTM.Model
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

        public void Add(ColorCondition cond)
        {
            _list.Add(cond);
        }

        public ColorCondition GetMatchColorCondition(string input)
        {
            foreach (var c in _list)
            {
                if (Regex.IsMatch(input, c.Pattern)) return c;
            }
            return null;
        }

        public void Remove(ColorCondition c)
        {
            _list.Remove(c);
        }

        public ColorCondition At(int i)
        {
            var index = 0;
            foreach (var c in _list)
            {
                if (index == i) return c;
                index++;
            }
            return null;
        }

        public override bool Equals(object obj)
        {
            var target = obj as ColorConditions;
            if (target == null) return false;
            if (_list.Count != target._list.Count) return false;
            for (var index = 0; index < _list.Count; index++)
            {
                if (!_list[index].Equals(target._list[index])) return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return _list.GetHashCode();
        }

        public void Up(int index)
        {
            var cond = _list[index];
            _list[index] = _list[index - 1];
            _list[index - 1] = cond;
        }

        public void Down(int index)
        {
            var cond = _list[index];
            _list[index] = _list[index + 1];
            _list[index + 1] = cond;
        }

        public void Apply(ColorConditions colorConditions)
        {
            _list.Clear();
            foreach (var c in colorConditions)
            {
                _list.Add(c);
            }
        }

        public ColorConditions Clone()
        {
            var result = new ColorConditions();
            _list.ForEach(c => result.Add(c.Clone()));
            return result;
        }
    }
}