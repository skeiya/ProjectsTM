using ProjectsTM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProjectsTM.ViewModel
{
    public class ViewData
    {
        public Filter Filter { get; private set; } = Filter.All;
        public Detail Detail { get; set; } = new Detail();

        public AppData Original => _appData;

        public void SetAppData(AppData appData, IUndoService undoService)
        {
            _appData = appData;
            UndoService = undoService;
            AddFreeTimeMembersToHideMembers(GetFilteredMembers());
            AppDataChanged?.Invoke(this, null);
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
            AddFreeTimeMembersToHideMembers(GetFilteredMembers());
            FilterChanged(this, null);
        }

        private bool Changed(Filter filter)
        {
            return !Filter.Equals(filter);
        }

        public IEnumerable<Member> GetFilteredMembers()
        {
            var result = CreateAllMembersList();
            return RemoveFilterSettingMembers(result);
        }

        private void AddFreeTimeMembersToHideMembers(IEnumerable<Member> members)
        {
            if (Filter.IsFreeTimeMemberShow) return;
            if (members == null || members.Count() > 0) return;
            var freeTimeMember = members.Where(m => !GetFilteredWorkItemsOfMember(m).HasWorkItem(Filter.Period));
            foreach (var m in freeTimeMember)
            {
                if (!Filter.HideMembers.Contains(m)) Filter.HideMembers.Add(m);
            }
        }

        private List<Member> RemoveFilterSettingMembers(List<Member> members)
        {
            return members.Where(m => !Filter.HideMembers.Contains(m)).ToList();
        }

        private List<Member> CreateAllMembersList()
        {
            return this.Original.Members.ToList();
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

        public IEnumerable<WorkItem> GetFilteredWorkItems()
        {
            var filteredMembers = GetFilteredMembers();
            var result = new WorkItems();
            foreach (var w in Original.WorkItems)
            {
                if (!filteredMembers.Contains(w.AssignedMember)) continue;
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
            if (!Filter.Period.IsValid) return Original.Callender.Days;
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
    }
}
