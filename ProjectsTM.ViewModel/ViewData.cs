using ProjectsTM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProjectsTM.ViewModel
{
    public class ViewData
    {
        public Filter Filter 
        { 
            get { return filter; } 
            private set 
            {
                filter = value;
                FilteredItems = new FilteredItems(Original, value); 
            } 
        }
        public FilteredItems FilteredItems { get; private set; }
        public Detail Detail { get; set; } = new Detail();

        public AppData Original => _appData;
        private Filter filter = Filter.All(null);

        public void SetAppData(AppData appData, IUndoService undoService)
        {
            _appData = appData;
            UndoService = undoService;
            UpdateFilter();
            UpdateShowMembers();
            AppDataChanged?.Invoke(this, null);
        }

        private void UpdateFilter()
        {
            if (!this.Filter.IsAllFilter) return;
            this.Filter = Filter.All(this);
        }

        private AppData _appData;
        public IUndoService UndoService { get; private set; }
        private WorkItems _selected;

        public void ClearCallenderAndMembers()
        {
            this.Original.Callender = new Callender();
            this.Original.Members = new Members();
        }

        public event EventHandler FilterChanged;
        public event EventHandler ColorConditionChanged;
        public event EventHandler<SelectedWorkItemChangedArg> SelectedWorkItemChanged;
        public event EventHandler AppDataChanged;

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

        public ViewData(AppData appData, IUndoService undoService)
        {
            SetAppData(appData, undoService);
        }

        public void SetFilter(Filter filter)
        {
            if (!Changed(filter)) return;
            Filter = filter;
            UpdateShowMembers();
            FilterChanged(this, null);
        }

        private bool Changed(Filter filter)
        {
            return !Filter.Equals(filter);
        }

        private void UpdateShowMembers()
        {
            RemoveAbsentMembersFromFilter();
            RemoveFreeTimeMembersFromFilter();
        }

        private void RemoveFreeTimeMembersFromFilter()
        {
            if (Filter.IsFreeTimeMemberShow) return;
            var members = FilteredItems.Members;
            if (members == null || members.Count() == 0) return;
            var freeTimeMember = members.Where(m => !GetFilteredWorkItemsOfMember(m).HasWorkItem(Filter.Period.IsValid ? Filter.Period : null));
            foreach (var m in freeTimeMember)
            {
                if (Filter.ShowMembers.Contains(m)) Filter.ShowMembers.Remove(m);
            }
        }

        private void RemoveAbsentMembersFromFilter()
        {
            var members = FilteredItems.Members;
            if (members == null || members.Count() == 0) return;
            Members absentMembers = new Members();
            foreach (var m in members)
            {
                var absentTerms = _appData.AbsentInfo.OfMember(m);
                if (!absentTerms.Any(a => (a.Period.Contains(this.Filter.Period)))) continue;
                absentMembers.Add(m);
            }
            foreach (var m in absentMembers)
            {
                if (Filter.ShowMembers.Contains(m)) Filter.ShowMembers.Remove(m);
            }
        }

        public WorkItem PickFilterdWorkItem(Member m, CallenderDay d)
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
            if (string.IsNullOrEmpty(Filter.WorkItem)) return false;
            return !Regex.IsMatch(w.ToString(), Filter.WorkItem);
        }

        public bool SelectNextWorkItem(bool prev)
        {
            if (Selected == null)
            {
                var all = FilteredItems.WorkItems.ToList();
                all.Sort();
                if (prev) all.Reverse();

                Selected = new WorkItems(all.FirstOrDefault());
                return true;
            }
            if (Selected.Count() == 1)
            {
                var all = FilteredItems.WorkItems.ToList();
                all.Sort();
                if (prev) all.Reverse();

                var find = all.FindIndex(wi => Selected.Unique.Equals(wi));
                WorkItem next = all.Skip(find + 1).FirstOrDefault();
                if (next == null) return false;

                Selected = new WorkItems(next);
                return true;
            }
            return false;
        }

        public void UpdateCallenderAndMembers(WorkItem wi)
        {
            var days = Original.Callender.Days;
            if (!days.Contains(wi.Period.From)) days.Add(wi.Period.From);
            if (!days.Contains(wi.Period.To)) days.Add(wi.Period.To);
            days.Sort();
            if (!Original.Members.Contains(wi.AssignedMember)) Original.Members.Add(wi.AssignedMember);
        }

        public void DecRatio()
        {
            if (Detail.ViewRatio <= 0.2) return;
            if (FontSize <= 1) return;
            FontSize--;
            Detail.ViewRatio -= 0.1f;
        }

        public void IncRatio()
        {
            FontSize++;
            Detail.ViewRatio += 0.1f;
        }

        public void SetColorConditions(ColorConditions colorConditions)
        {
            if (Original.ColorConditions.Equals(colorConditions)) return;
            Original.ColorConditions = colorConditions;
            ColorConditionChanged?.Invoke(this, null);
        }
    }
}
