using ProjectsTM.Model;
using System;
using System.Linq;

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
                UpdateFilteredItems();
            }
        }
        public FilteredItems FilteredItems { get; private set; }

        public AppData Original => _appData;
        private Filter filter = Filter.All(null);

        public void SetAppData(AppData appData)
        {
            if (appData == null) return;
            _appData = appData;
            UndoService.Clear();
            UpdateFilter();
            UpdateFilteredItems();
            UpdateShowMembers();
            Selected = null;
            AppDataChanged?.Invoke(this, null);
        }

        private void UpdateFilteredItems()
        {
            FilteredItems = new FilteredItems(Original, filter);
        }

        private void UpdateFilter()
        {
            if (!this.Filter.IsAllFilter) return;
            this.Filter = Filter.All(this);
        }

        private AppData _appData;
        public IUndoService UndoService { get; private set; }
        private WorkItems _selected;

        public event EventHandler FilterChanged;
        public event EventHandler<SelectedWorkItemChangedArg> SelectedWorkItemChanged;
        public event EventHandler AppDataChanged;


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
            UndoService = undoService;
            SetAppData(appData);
        }

        public void SetFilter(Filter filter)
        {
            if (!Changed(filter)) return;
            Filter = filter;
            UpdateShowMembers();
            FilterChanged?.Invoke(this, null);
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
            if (members == null || !members.Any()) return;
            var freeTimeMember = members.Where(m => !FilteredItems.GetWorkItemsOfMember(m).HasWorkItem(Filter.Period.IsValid ? Filter.Period : null));
            foreach (var m in freeTimeMember)
            {
                if (Filter.ShowMembers.Contains(m)) Filter.ShowMembers.Remove(m);
            }
        }

        private void RemoveAbsentMembersFromFilter()
        {
            var members = FilteredItems.Members;
            if (members == null || !members.Any()) return;
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

        public void SetColorConditions(ColorConditions colorConditions)
        {
            if (Original.ColorConditions.Equals(colorConditions)) return;
            Original.ColorConditions = colorConditions;
        }
    }
}
