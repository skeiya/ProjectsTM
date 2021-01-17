using ProjectsTM.Model;
using System.Drawing;

namespace ProjectsTM.UI.TaskList
{
    internal class TaskListItem
    {
        public TaskListItem(WorkItem w, Color color, string errMsg)
        {
            this.WorkItem = w;
            this.Color = color;
            IsMilestone = false;
            ErrMsg = string.IsNullOrEmpty(errMsg) ? string.Empty : errMsg;
        }

        public TaskListItem(WorkItem w, MileStone mileStone, Color color, string errMsg)
        {
            this.WorkItem = w;
            this.Color = color;
            this.MileStone = mileStone;
            IsMilestone = true;
            ErrMsg = string.IsNullOrEmpty(errMsg) ? string.Empty : errMsg;
        }

        public WorkItem WorkItem { get; internal set; }

        public MileStone MileStone { get; set; }
        public Color Color { get; private set; }
        public bool IsMilestone { get; internal set; }
        public string ErrMsg { get; }
    }
}