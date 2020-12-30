using ProjectsTM.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace ProjectsTM.ViewModel
{
    public class FilteredItems
    {
        private readonly AppData _appData;
        private readonly Filter _filter;

        public IEnumerable<Member> Members
        {
            get 
            {
                var result = CreateAllMembersList();
                return GetFilterShowMembers(result);
            }
        }

        public List<CallenderDay> Days
        {
            get
            {
                if (!_filter.Period.IsValid) return _appData.Callender.Days;
                var result = new List<CallenderDay>();
                bool isFound = false;
                foreach (var d in _appData.Callender.Days)
                {
                    if (d.Equals(_filter.Period.From)) isFound = true;
                    if (isFound) result.Add(d);
                    if (d.Equals(_filter.Period.To)) return result;
                }
                return result;
            }
        }


        public IEnumerable<WorkItem> WorkItems
        {
            get
            {
                var filteredMembers = Members;
                var result = new WorkItems();
                foreach (var w in _appData.WorkItems)
                {
                    if (!filteredMembers.Contains(w.AssignedMember)) continue;
                    if (!string.IsNullOrEmpty(_filter.WorkItem))
                    {
                        if (!Regex.IsMatch(w.ToString(), _filter.WorkItem)) continue;
                    }
                    if (!w.Period.HasInterSection(_filter.Period)) continue;
                    result.Add(w);
                }
                return result;
            }
        }

        public FilteredItems(AppData appData, Filter filter)
        {
            _appData = appData;
            _filter = filter;
        }

        private List<Member> CreateAllMembersList()
        {
            return this._appData.Members.ToList();
        }

        private List<Member> GetFilterShowMembers(List<Member> members)
        {
            return members.Where(m => _filter.ShowMembers.Contains(m)).ToList();
        }

        public WorkItem PickWorkItem(Member m, CallenderDay d)
        {
            if (m == null) return null;
            foreach (var wi in GetWorkItemsOfMember(m))
            {
                if (wi.Period.Contains(d)) return wi;
            }
            return null;
        }

        public MembersWorkItems GetWorkItemsOfMember(Member m)
        {
            var result = new MembersWorkItems();
            foreach (var w in _appData.WorkItems.OfMember(m))
            {
                if (!string.IsNullOrEmpty(_filter.WorkItem))
                {
                    if (IsFilteredWorkItem(w)) continue;
                }
                result.Add(w);
            }
            return result;
        }

        private bool IsFilteredWorkItem(WorkItem w)
        {
            if (string.IsNullOrEmpty(_filter.WorkItem)) return false;
            return !Regex.IsMatch(w.ToString(), _filter.WorkItem);
        }
    }
}
