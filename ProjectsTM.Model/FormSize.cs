using System.Collections.Generic;

namespace ProjectsTM.Model
{
    public class FormSize
    {
        public int SearchWorkitemFormHeight { set; get; }
        public int TaskListFormHeight { set; get; }
        public List<int> TaskListColWidths { set; get; } = new List<int>();
        public FormSize() { }
    }
}
