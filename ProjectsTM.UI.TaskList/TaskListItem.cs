using ProjectsTM.Model;
using System.Drawing;

namespace ProjectsTM.UI.TaskList
{
    internal class TaskListItem
    {
        public TaskListItem(WorkItem w, Color color, bool isMilestone, string errMsg)
        {
            this.WorkItem = w;
            this.Color = color;
            IsMilestone = isMilestone;
            ErrMsg = string.IsNullOrEmpty(errMsg) ? string.Empty : errMsg;
        }

        public WorkItem WorkItem { get;  internal set; }
        public Color Color { get; private set; }
        public bool IsMilestone { get; internal set; }
        public string ErrMsg { get; }

        public bool Equals(MileStone ms)
        {
            if (!IsMilestone) return false;
            if (WorkItem.Name != ms.Name) return false;
            if (WorkItem.Period.From != ms.Day) return false;
            if (WorkItem.Period.To != ms.Day) return false;
            if (WorkItem.Project != ms.Project) return false;
            if (WorkItem.State != ms.State) return false;
            return true;
        }
    }
}