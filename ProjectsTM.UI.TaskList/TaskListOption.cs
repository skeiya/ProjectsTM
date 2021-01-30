namespace ProjectsTM.UI.TaskList
{
    public class TaskListOption
    {
        public string Pattern;
        public string AndPattern;
        public bool IsShowMS = true;
        public ErrorDisplayType ErrorDisplayType = ErrorDisplayType.All;

        public TaskListOption()
        {
        }

        public TaskListOption(string pattern, bool isShowMS, string andPattern, ErrorDisplayType errorDisplayType)
        {
            Pattern = pattern;
            IsShowMS = isShowMS;
            AndPattern = andPattern;
            ErrorDisplayType = errorDisplayType;
        }
    }
}
