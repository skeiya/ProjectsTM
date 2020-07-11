using ProjectsTM.Model;
using System.Drawing;

namespace ProjectsTM.UI
{
    internal class TaskListItem
    {
        public TaskListItem(WorkItem w, Color color)
        {
            this.WorkItem = w;
            this.Color = color;
        }

        public WorkItem WorkItem { get; private set; }
        public Color Color { get; private set; }
    }
}