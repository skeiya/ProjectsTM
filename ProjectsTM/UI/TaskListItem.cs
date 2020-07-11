using ProjectsTM.Model;
using System.Drawing;

namespace ProjectsTM.UI
{
    internal class TaskListItem
    {
        public TaskListItem(WorkItem w, Color color, bool isMilestone)
        {
            this.WorkItem = w;
            this.Color = color;
            IsMilestone = isMilestone;
        }

        public WorkItem WorkItem { get; private set; }
        public Color Color { get; private set; }
        public bool IsMilestone { get; internal set; }
    }
}