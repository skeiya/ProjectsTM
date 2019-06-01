using System.Collections.Generic;
using System.Linq;
using TaskManagement.Model;
using TaskManagement.ViewModel;

namespace TaskManagement.Service
{
    public class CalculateSumService
    {
        private Dictionary<Member, int> _sumCache = new Dictionary<Member, int>();

        public int Calculate(ViewData _viewData, List<Member> updatedMembers)
        {
            UpdateCache(_viewData, updatedMembers);
            var filteredMembers = _viewData.GetFilteredMembers();
            return _sumCache.Where((i) => filteredMembers.Contain(i.Key)).Sum((i) => i.Value);
        }

        private void UpdateCache(ViewData _viewData, List<Member> updatedMembers)
        {
            ClearDirtyCache(updatedMembers);
            UpdateBlankCache(_viewData);
        }

        private void UpdateBlankCache(ViewData _viewData)
        {
            foreach (var m in _viewData.Original.Members) UpdateMemberCache(_viewData, m);
        }

        private void ClearDirtyCache(List<Member> updatedMembers)
        {
            if (updatedMembers == null)
            {
                _sumCache.Clear();
            }
            else
            {
                foreach (var m in updatedMembers) _sumCache.Remove(m);
            }
        }

        private void UpdateMemberCache(ViewData _viewData, Member m)
        {
            if (!_sumCache.ContainsKey(m))
            {
                _sumCache.Add(m, CalculateMember(_viewData, m));
            }
        }

        private static int CalculateMember(ViewData _viewData, Member m)
        {
            var sumOfMember = 0;
            foreach (var w in _viewData.GetFilteredWorkItemsOfMember(m))
            {
                sumOfMember += _viewData.Original.Callender.GetPeriodDayCount(w.Period);
            }

            return sumOfMember;
        }
    }
}
