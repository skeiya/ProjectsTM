﻿using ProjectsTM.Model;
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

        public WorkItem WorkItem { get; private set; }
        public Color Color { get; private set; }
        public bool IsMilestone { get; internal set; }
        public string ErrMsg { get; }
    }
}