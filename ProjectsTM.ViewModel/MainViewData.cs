using ProjectsTM.Model;
using System;

namespace ProjectsTM.ViewModel
{
    public class MainViewData
    {
        private readonly ViewData _viewData;

        public event EventHandler FilterChanged;
        public event EventHandler<SelectedWorkItemChangedArg> SelectedWorkItemChanged;
        public event EventHandler AppDataChanged;
        public event EventHandler RatioChanged;

        public MainViewData(AppData appData)
        {
            _viewData = new ViewData(appData);
            _viewData.FilterChanged += (s, e) => this.FilterChanged?.Invoke(s, e);
            _viewData.SelectedWorkItemChanged += (s, e) => this.SelectedWorkItemChanged?.Invoke(s, e);
            _viewData.AppDataChanged += (s, e) => this.AppDataChanged?.Invoke(s, e);
        }

        public AppData Original => _viewData.Original;

        public UndoBuffer UndoBuffer => _viewData.UndoBuffer;

        public WorkItems Selected
        {
            get
            {
                return _viewData.Selected;
            }
            set
            {
                _viewData.Selected = value;
            }
        }

        public FilteredItems FilteredItems => _viewData.FilteredItems;

        public Filter Filter => _viewData.Filter;

        public ViewData Core => _viewData;

        public void SetColorConditions(ColorConditions colorConditions)
        {
            _viewData.SetColorConditions(colorConditions);
        }

        public void SetAppData(AppData appData)
        {
            _viewData.SetAppData(appData);
        }

        public void SetFilter(Filter filter)
        {
            _viewData.SetFilter(filter);
        }

        public int FontSize { get; set; } = 6;

        public void DecRatio()
        {
            if (Detail.ViewRatio <= 0.2) return;
            if (FontSize <= 1) return;
            FontSize--;
            Detail.ViewRatio -= 0.1f;
            RatioChanged?.Invoke(this, null);
        }

        public void IncRatio()
        {
            FontSize++;
            Detail.ViewRatio += 0.1f;
            RatioChanged?.Invoke(this, null);
        }

        public Detail Detail { get; set; } = new Detail();
    }
}
