using ProjectsTM.Model;

namespace ProjectsTM.Service
{
    public class EditAction
    {
        private readonly EditActionType _action;
        private readonly string _workItem;
        private readonly Member _member;

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
