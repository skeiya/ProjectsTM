using ProjectsTM.Model;
using ProjectsTM.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace ProjectsTM.Service
{
    public class CalculateSumService
    {
        private readonly Dictionary<Member, int> _sumCache = new Dictionary<Member, int>();

        public int Calculate(ViewData viewData, List<Member> updatedMembers)
        {
            UpdateCache(viewData, updatedMembers);
            var filteredMembers = viewData.FilteredItems.Members;
            return _sumCache.Where((i) => filteredMembers.Contains(i.Key)).Sum((i) => i.Value);
        }

        private void UpdateCache(ViewData viewData, List<Member> updatedMembers)
        {
            ClearDirtyCache(updatedMembers);
            UpdateBlankCache(viewData);
        }

        private void UpdateBlankCache(ViewData viewData)
        {
            foreach (var m in viewData.Original.Members) UpdateMemberCache(viewData, m);
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

        private void UpdateMemberCache(ViewData viewData, Member m)
        {
            if (!_sumCache.ContainsKey(m))
            {
                _sumCache.Add(m, CalculateMember(viewData, m));
            }
        }

        private static int CalculateMember(ViewData viewData, Member m)
        {
            var sumOfMember = 0;
            foreach (var w in viewData.FilteredItems.GetWorkItemsOfMember(m))
            {
                sumOfMember += viewData.Original.Callender.GetPeriodDayCount(w.Period);
            }

            return sumOfMember;
        }
    }
}
