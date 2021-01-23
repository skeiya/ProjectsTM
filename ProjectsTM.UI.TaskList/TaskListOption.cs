namespace ProjectsTM.UI.TaskList
{
    public class TaskListOption
    {
        public string Pattern;
        public string AndPattern;
        public bool IsShowMS = true;
        public bool IsShowOverwrap = false;

        public TaskListOption()
        {
        }

        public TaskListOption(string pattern, bool isShowMS, string andPattern, bool isShowOverwrap)
        {
            Pattern = pattern;
            IsShowMS = isShowMS;
            AndPattern = andPattern;
            IsShowOverwrap = isShowOverwrap;
        }
    }
}
