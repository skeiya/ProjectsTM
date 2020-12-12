using FreeGridControl;
using ProjectsTM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectsTM.UI.TaskList
{
    class ColDefinition
    {
        public static int Count => Enum.GetNames(typeof(ColIds)).Count();

        public static ColIndex InitialSortCol => ToIndex(ColIds.End);

        public static ColIndex AutoExtendCol => ToIndex(ColIds.Description);

        private static Dictionary<ColIds, ColSpecification> _colTable = new Dictionary<ColIds, ColSpecification>() {
            {ColIds.Name, new ColSpecification("名前", (i,cal)=>i.WorkItem.Name) },
            {ColIds.Error, new ColSpecification("エラー", (i, cal) => i.ErrMsg) },
            {ColIds.Project, new ColSpecification( "プロジェクト", (i, cal) => i.WorkItem.Project.ToString()) },
            {ColIds.Assign, new ColSpecification( "担当", (i, cal) => i.WorkItem.AssignedMember.ToString()) },
            {ColIds.Tag, new ColSpecification( "タグ", (i, cal) => i.WorkItem.Tags.ToString()) },
            {ColIds.State, new ColSpecification( "状態", (i,cal) => i.WorkItem.State.ToString())},
            {ColIds.Start, new ColSpecification( "開始", (i, cal) => i.WorkItem.Period.From.ToString()) },
            {ColIds.End, new ColSpecification( "終了", (i, cal) => i.WorkItem.Period.To.ToString()) },
            {ColIds.DayCount, new ColSpecification( "人日", (i, cal) =>  cal.GetPeriodDayCount(i.WorkItem.Period).ToString())},
            {ColIds.Description, new ColSpecification( "備考", (i, cal) => i.WorkItem.Description) },
        };

        public static string GetTitle(ColIndex c)
        {
            return _colTable[ToId(c)].Title;
        }

        private static ColIndex ToIndex(ColIds id)
        {
            var idx = (int)id;
            return new ColIndex(idx);
        }

        private static ColIds ToId(ColIndex c)
        {
            return (ColIds)c.Value;
        }

        public static string GetText(TaskListItem item, ColIndex c, ViewData viewData)
        {
            return _colTable[ToId(c)].GetText(item, viewData.Original.Callender);
        }

        private static bool IsDayCountCol(ColIndex c)
        {
            return ToIndex(ColIds.DayCount).Equals(c);
        }

        private static bool IsErrorCol(ColIndex c)
        {
            return ToIndex(ColIds.Error).Equals(c);
        }

        internal static void Sort(ColIndex sortCol, ref List<TaskListItem> listItems, ViewData viewData)
        {
            if (IsDayCountCol(sortCol))
            {
                listItems = listItems.OrderBy(l => viewData.Original.Callender.GetPeriodDayCount(l.WorkItem.Period)).ToList();
            }
            else if (IsErrorCol(sortCol))
            {
                var te = GetText(listItems[0], sortCol, viewData);
                var temp = GetText(listItems[3], sortCol, viewData);
                listItems = listItems.OrderByDescending(l => GetText(l, sortCol, viewData)).ToList();
            }
            else
            {
                listItems = listItems.OrderBy(l => GetText(l, sortCol, viewData)).ToList();
            }
        }
    }
}
