﻿using System;
using System.Collections.Generic;
using ProjectsTM.Model;
using ProjectsTM.ViewModel;

namespace ProjectsTM.Service
{
    public class UndoService
    {
        private Stack<AtomicAction> _undoStack = new Stack<AtomicAction>();
        private Stack<AtomicAction> _redoStack = new Stack<AtomicAction>();

        public event EventHandler<EditedEventArgs> Changed;

        public UndoService()
        {
        }

        private AtomicAction _atomicAction = new AtomicAction();

        internal void Delete(WorkItems wis)
        {
            foreach (var w in wis) Delete(w);
        }

        internal void Delete(WorkItem w)
        {
            _atomicAction.Add(new EditAction(EditActionType.Delete, w.Serialize(), w.AssignedMember));
        }

        internal void Add(WorkItems wis)
        {
            foreach (var w in wis) Add(w);
        }

        internal void Add(WorkItem w)
        {
            _atomicAction.Add(new EditAction(EditActionType.Add, w.Serialize(), w.AssignedMember));
        }

        internal void Push()
        {
            _undoStack.Push(_atomicAction.Clone());
            Changed?.Invoke(this, new EditedEventArgs(_atomicAction.Members));
            _atomicAction.Clear();
            _redoStack.Clear();
        }

        internal void Undo(ViewData viewData)
        {
            if (_undoStack.Count == 0) return;
            var p = _undoStack.Pop();
            _redoStack.Push(p);
            viewData.Selected = new WorkItems();
            foreach (var a in p)
            {
                var w = WorkItem.Deserialize(a.WorkItemText);
                if (a.Action == EditActionType.Add)
                {
                    viewData.Original.WorkItems.Remove(w);
                }
                else if (a.Action == EditActionType.Delete)
                {
                    viewData.Original.WorkItems.Add(w);
                    viewData.Selected.Add(w);
                }
            }
            Changed(this, new EditedEventArgs(p.Members));
        }

        internal void Redo(ViewData viewData)
        {
            if (_redoStack.Count == 0) return;
            var r = _redoStack.Pop();
            _undoStack.Push(r);
            viewData.Selected = new WorkItems();
            foreach (var a in r)
            {
                var w = WorkItem.Deserialize(a.WorkItemText);
                if (a.Action == EditActionType.Add)
                {
                    viewData.Original.WorkItems.Add(w);
                    viewData.Selected.Add(w);
                }
                else if (a.Action == EditActionType.Delete)
                {
                    viewData.Original.WorkItems.Remove(w);
                }
            }
            Changed(this, new EditedEventArgs(r.Members));
        }
    }
}