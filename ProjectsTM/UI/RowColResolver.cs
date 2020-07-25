﻿using FreeGridControl;
using ProjectsTM.Model;
using ProjectsTM.ViewModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ProjectsTM.UI
{
    class RowColResolver
    {
        private Dictionary<CallenderDay, RowIndex> _day2RowCache = new Dictionary<CallenderDay, RowIndex>();
        private Dictionary<RowIndex, CallenderDay> _row2DayChache = new Dictionary<RowIndex, CallenderDay>();
        private Dictionary<Member, ColIndex> _member2ColChache = new Dictionary<Member, ColIndex>();
        private Dictionary<ColIndex, Member> _col2MemberChache = new Dictionary<ColIndex, Member>();
        private readonly GridControl _grid;
        private readonly ViewData _viewData;

        public RowColResolver(GridControl grid, ViewData viewData)
        {
            this._grid = grid;
            this._viewData = viewData;
        }
        public void ClearCache()
        {
            _day2RowCache.Clear();
            _row2DayChache.Clear();
            _member2ColChache.Clear();
            _col2MemberChache.Clear();
        }

        internal CallenderDay Row2Day(RowIndex r)
        {
            if (_row2DayChache.TryGetValue(r, out var day)) return day;
            if (r == null) return null;
            var days = _viewData.GetFilteredDays();
            if (r.Value - _grid.FixedRowCount < 0 || days.Count <= r.Value - _grid.FixedRowCount) return null;
            day = days.ElementAt(r.Value - _grid.FixedRowCount);
            _row2DayChache.Add(r, day);
            return day;
        }

        internal ColIndex Member2Col(Member m, IEnumerable<Member> members)
        {
            if (_member2ColChache.TryGetValue(m, out var col)) return col;
            foreach (var c in ColIndex.Range(0, _grid.ColCount))
            {
                if (members.ElementAt(c.Value).Equals(m))
                {
                    var result = c.Offset(_grid.FixedColCount);
                    _member2ColChache.Add(m, result);
                    return result;
                }
            }
            Debug.Assert(false);
            return null;
        }

        internal RowIndex Day2Row(CallenderDay day)
        {
            if (_day2RowCache.TryGetValue(day, out var row)) return row;
            foreach (var r in RowIndex.Range(_grid.FixedRowCount, _grid.RowCount - _grid.FixedRowCount))
            {
                if (_viewData.GetFilteredDays().ElementAt(r.Value - _grid.FixedRowCount).Equals(day))
                {
                    _day2RowCache.Add(day, r);
                    return r;
                }
            }
            return null;
        }

        internal Member Col2Member(ColIndex c)
        {
            if (_col2MemberChache.TryGetValue(c, out var member)) return member;
            if (c == null) return null;
            var members = _viewData.GetFilteredMembers();
            if (c.Value - _grid.FixedColCount < 0 || members.Count() <= c.Value - _grid.FixedColCount) return null;
            var result = _viewData.GetFilteredMembers().ElementAt(c.Value - _grid.FixedColCount);
            _col2MemberChache.Add(c, result);
            return result;
        }
    }
}