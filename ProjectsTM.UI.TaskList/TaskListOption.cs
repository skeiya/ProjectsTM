﻿namespace ProjectsTM.UI.TaskList
{
    public class TaskListOption
    {
        public string Pattern;
        public string AndPattern;
        public bool IsShowMS = true;

        public TaskListOption()
        {
        }

        public TaskListOption(string pattern, bool isShowMS, string andPattern)
        {
            Pattern = pattern;
            IsShowMS = isShowMS;
            AndPattern = andPattern;
        }
    }
}