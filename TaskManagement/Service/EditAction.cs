using TaskManagement.Model;

namespace TaskManagement.Service
{
    class EditAction
    {
        private EditActionType _action;
        private string _workItem;
        private Member _member;

        public EditActionType Action => _action;
        public string WorkItemText => _workItem;
        public Member Member => _member;

        public EditAction(EditActionType action, string workItem, Member member)
        {
            this._action = action;
            this._workItem = workItem;
            this._member = member;
        }
    }
}
