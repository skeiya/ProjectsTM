using FreeGridControl;
using ProjectsTM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsTM.UI.TaskList
{
    class ColDefinition
    {
        public static int Count => 10;

        public static ColIndex InitialSortCol => new ColIndex(7);

        public static string GetTitle(ColIndex c)
        {
            string[] titles = new string[] { "名前", "エラー", "プロジェクト", "担当", "タグ", "状態", "開始", "終了", "人日", "備考" };
            return titles[c.Value];
        }

        public static string GetText(TaskListItem item, ColIndex c, ViewData viewData)
        {
            var colIndex = c.Value;
            var wi = item.WorkItem;
            if (colIndex == 0)
            {
                return wi.Name;
            }
            else if (colIndex == 1)
            {
                return item.ErrMsg;
            }
            else if (colIndex == 2)
            {
                return wi.Project.ToString();
            }
            else if (colIndex == 3)
            {
                return wi.AssignedMember.ToString();
            }
            else if (colIndex == 4)
            {
                return wi.Tags.ToString();
            }
            else if (colIndex == 5)
            {
                return wi.State.ToString();
            }
            else if (colIndex == 6)
            {
                return wi.Period.From.ToString();
            }
            else if (colIndex == 7)
            {
                return wi.Period.To.ToString();
            }
            else if (colIndex == 8)
            {
                return viewData.Original.Callender.GetPeriodDayCount(wi.Period).ToString();
            }
            else if (colIndex == 9)
            {
                return wi.Description;
            }
            return string.Empty;
        }

        private static bool IsDayCountCol(ColIndex c)
        {
            return c.Value == 8;
        }

        internal static void Sort(ColIndex sortCol, List<TaskListItem> listItems, ViewData viewData)
        {
            if (IsDayCountCol(sortCol))
            {
                listItems = listItems.OrderBy(l => viewData.Original.Callender.GetPeriodDayCount(l.WorkItem.Period)).ToList();
            }
            else
            {
                listItems = listItems.OrderBy(l => GetText(l, sortCol, viewData)).ToList();
            }
        }
    }
}
