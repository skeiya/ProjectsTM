using ProjectsTM.Model;

namespace ProjectsTM.ViewModel
{
    public class DragDoneEventArgs
    {
        public DragDoneEventArgs(WorkItems before, WorkItems after)
        {
            this.Before = before;
            this.After = after;
        }

        public WorkItems Before{ get; }
        public WorkItems After { get; }
    }
}
