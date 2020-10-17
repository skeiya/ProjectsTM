using ProjectsTM.Model;
using System;

namespace ProjectsTM.UI.TaskList
{
    class ColSpecification
    {
        public string Title;
        public Func<TaskListItem, Callender, string> GetText;

        public ColSpecification(string title, Func<TaskListItem, Callender, string> getText)
        {
            Title = title;
            GetText = getText;
        }
    }
}
