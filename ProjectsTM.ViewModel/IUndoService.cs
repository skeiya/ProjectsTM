using ProjectsTM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsTM.ViewModel
{
    public interface IUndoService
    {
        void Add(WorkItem w);
        void Push();
        void Delete(WorkItems selected);
        void Add(WorkItems after);
        void Delete(WorkItem selected);

        event EventHandler<IEditedEventArgs> Changed;

        void Redo(ViewData viewData);
        void Undo(ViewData viewData);
    }
}
