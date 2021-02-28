using FreeGridControl;
using ProjectsTM.Model;
using System.Collections.Generic;
using System.Drawing;

namespace ProjectsTM.ViewModel
{
    public interface IWorkItemGrid
    {
        Member X2Member(int x);
        ColIndex X2Col(int x);
        Member Col2Member(ColIndex c);
        ColIndex Member2Col(Member m);
        CallenderDay Y2Day(int y);
        CallenderDay Row2Day(RowIndex r);
        RowIndex Y2Row(int y);

        Rectangle? GetMemberDrawRect(Member m);
        RawRectangle? GetRectRaw(ColIndex col, RowIndex r, int rowCount);
        ClientRectangle? GetRectClient(ColIndex col, RowIndex r, int rowCount, ClientRectangle visibleArea);
        RawRectangle? GetWorkItemDrawRectRaw(WorkItem wi, IEnumerable<Member> members);
        ClientRectangle? GetWorkItemDrawRectClient(WorkItem wi, IEnumerable<Member> members);
        IEnumerable<ClientRectangle?> GetWorkItemDrawRectClient(WorkItems wis, IEnumerable<Member> members);
        IEnumerable<Member> GetNeighbers(IEnumerable<Member> members);


        Size FullSize { get; }
        Size VisibleSize { get; }
        Size FixedSize { get; }
        Point ScrollOffset { get; }
        RowColRange VisibleRowColRange { get; }
        ClientRectangle GetVisibleRect(bool isFixedRow, bool isFixedCol);

        RawPoint Global2Raw(Point global);

        ClientRectangle? GetRangeSelectBound();
        bool IsSelected(Member m);
        bool IsSelected(CallenderDay d);
        void Invalidate();
#pragma warning disable CA1021 // out パラメーターを使用しません
        bool PickWorkItemFromPoint(RawPoint curOnRaw, out WorkItem result);
#pragma warning restore CA1021 // out パラメーターを使用しません
        bool IsFixedArea(ClientPoint location);
        RawPoint Client2Raw(ClientPoint location);
        void EditSelectedWorkItem();
        void AddNewWorkItem(WorkItem proto);
    }
}
