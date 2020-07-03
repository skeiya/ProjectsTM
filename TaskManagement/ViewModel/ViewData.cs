﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TaskManagement.Model;

namespace TaskManagement.ViewModel
{
    public class ViewData
    {
        public Filter Filter { get; private set; }
        public Detail Detail { get; set; } = new Detail();
        public bool EnableFreeTimeMember = false;

        public AppData Original
        {
            get { return _appData; }
            set
            {
                _appData = value;
                if (AppDataChanged != null) AppDataChanged(this, null);
            }
        }

        private AppData _appData;
        private WorkItems _selected;

        internal void ClearCallenderAndMembers()
        {
            this.Original.Callender = new Callender();
            this.Original.Members = new Members();
        }

        public event EventHandler FilterChanged;
        public event EventHandler<SelectedWorkItemChangedArg> SelectedWorkItemChanged;
        public event EventHandler AppDataChanged;
        public event EventHandler FontChanged;

        public int FontSize { set; get; } = 6;

        public WorkItems Selected
        {
            get { return _selected; }
            set
            {
                var org = _selected;
                _selected = value;
                if (_selected != org)
                {
                    var arg = new SelectedWorkItemChangedArg(org, _selected);
                    SelectedWorkItemChanged?.Invoke(this, arg);
                }
            }
        }

        public ViewData(AppData appData)
        {
            Original = appData;
        }

        public void SetFilter(Filter filter)
        {
            if (!Changed(filter)) return;
            Filter = filter;
            FilterChanged(this, null);
        }

        private bool Changed(Filter filter)
        {
            if (Filter == null && filter == null) return false;
            if (Filter != null) return !Filter.Equals(filter);
            return !filter.Equals(Filter);
        }

        public int GetDaysCount()
        {
            if (Filter == null || Filter.Period == null) return Original.Callender.Days.Count;
            return Original.Callender.GetPeriodDayCount(Filter.Period);
        }

        public Members GetFilteredMembers()
        {
            var result = new Members();
            if (Filter == null || Filter.HideMembers == null)
            {
                foreach (var m in this.Original.Members)
                {
                    var workItems = GetFilteredWorkItemsOfMember(m);
                    if (!EnableFreeTimeMember &&
                        !workItems.HasWorkItem(Filter == null ? null : Filter.Period)) continue;
                    result.Add(m);
                }
                return result;
            }

            foreach (var m in Original.Members)
            {
                if (Filter.HideMembers.Contain(m)) continue;
                var workItems = GetFilteredWorkItemsOfMember(m);
                if (!EnableFreeTimeMember && !workItems.HasWorkItem(Filter.Period)) continue;
                result.Add(m);
            }
            return result;
        }

        internal WorkItem PickFilterdWorkItem(Member m, CallenderDay d)
        {
            if (m == null) return null;
            foreach (var wi in GetFilteredWorkItemsOfMember(m))
            {
                if (wi.Period.Contains(d)) return wi;
            }
            return null;
        }

        public MembersWorkItems GetFilteredWorkItemsOfMember(Member m)
        {
            if (Filter == null) return Original.WorkItems.OfMember(m);
            var result = new MembersWorkItems();
            foreach (var w in Original.WorkItems.OfMember(m))
            {
                if (!string.IsNullOrEmpty(Filter.WorkItem))
                {
                    if (IsFilteredWorkItem(w)) continue;
                }
                result.Add(w);
            }
            return result;
        }

        public bool IsFilteredWorkItem(WorkItem w)
        {
            if (Filter == null || Filter.WorkItem == null) return false;
            return !Regex.IsMatch(w.ToString(), Filter.WorkItem);
        }

        public WorkItems GetFilteredWorkItems()
        {
            if (Filter == null) return Original.WorkItems;
            var filteredMembers = GetFilteredMembers();
            var result = new WorkItems();
            foreach (var w in Original.WorkItems)
            {
                if (!filteredMembers.Contain(w.AssignedMember)) continue;
                if (!string.IsNullOrEmpty(Filter.WorkItem))
                {
                    if (!Regex.IsMatch(w.ToString(), Filter.WorkItem)) continue;
                }
                if (!w.Period.HasInterSection(Filter.Period)) continue;
                result.Add(w);
            }
            return result;
        }

        public List<CallenderDay> GetFilteredDays()
        {
            if (Filter == null) return Original.Callender.Days;
            if (Filter.Period == null) return Original.Callender.Days;
            var result = new List<CallenderDay>();
            bool isFound = false;
            foreach (var d in Original.Callender.Days)
            {
                if (d.Equals(Filter.Period.From)) isFound = true;
                if (isFound) result.Add(d);
                if (d.Equals(Filter.Period.To)) return result;
            }
            return result;
        }

        public void UpdateCallenderAndMembers(WorkItem wi)
        {
            var days = Original.Callender.Days;
            if (!days.Contains(wi.Period.From)) days.Add(wi.Period.From);
            if (!days.Contains(wi.Period.To)) days.Add(wi.Period.To);
            days.Sort();
            if (!Original.Members.Contain(wi.AssignedMember)) Original.Members.Add(wi.AssignedMember);
        }

        internal void IncFont()
        {
            FontSize++;
            FontChanged(this, null);
        }

        internal void DecFont()
        {
            if (FontSize <= 1) return;
            FontSize--;
            FontChanged(this, null);
        }

        internal void DecRatio()
        {
            if (Detail.ViewRatio <= 0.2) return;
            Detail.ViewRatio -= 0.1f;
        }

        internal void IncRatio()
        {
            Detail.ViewRatio += 0.1f;
        }
    }
}
