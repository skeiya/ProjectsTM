﻿namespace ProjectsTM.UI.TaskList
{
    public class TaskListOption
    {
        public string Pattern;
        public string AndPattern;
        public bool IsShowMS = true;
        public bool IsShowOverlap = false;

        public TaskListOption()
        {
        }

        public TaskListOption(string pattern, bool isShowMS, string andPattern, bool isShowOverlap)
        {
            Pattern = pattern;
            IsShowMS = isShowMS;
            AndPattern = andPattern;
            IsShowOverlap = isShowOverlap;
        }
    }
}
