namespace ProjectsTM.UI.TaskList
{
    class TaskListOption
    {
        public string Pattern;
        public string AndPattern;
        public bool IsShowMS;

        public TaskListOption(string pattern, bool isShowMS, string andPattern)
        {
            Pattern = pattern;
            IsShowMS = isShowMS;
            AndPattern = andPattern;
        }
    }
}
