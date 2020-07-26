using ProjectsTM.Model;
using System;

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
