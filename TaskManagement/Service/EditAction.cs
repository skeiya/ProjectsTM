using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Service
{
    class EditAction
    {
        private EditActionType _action;
        private string _workItem;

        public EditActionType Action => _action;
        public string WorkItemText => _workItem;

        public EditAction(EditActionType action, string workItem)
        {
            this._action = action;
            this._workItem = workItem;
        }
    }
}
