using FreeGridControl;
using System.Collections.Generic;
using System.Drawing;
using TaskManagement.Model;
using TaskManagement.UI;

namespace TaskManagement.ViewModel
{
    interface IWorkItemGrid
    {
        Member X2Member(int x);
        ColIndex X2Col(float x);
        Member Col2Member(ColIndex c);
        ColIndex Member2Col(Member m);
        CallenderDay Y2Day(int y);
        CallenderDay Row2Day(RowIndex r);
        RowIndex Y2Row(float y);

        RectangleF? GetMemberDrawRect(Member m);
        RectangleF? GetRect(ColIndex col, RowIndex r, int rowCount, bool isFixedRow, bool isFixedCol, bool isFrontView);
        RectangleF? GetWorkItemDrawRect(WorkItem wi, Members members, bool isFrontView);
        IEnumerable<Member> GetNeighbers(IEnumerable<Member> members);


        SizeF FullSize { get; }
        Size VisibleSize { get; }
        SizeF FixedSize { get; }
        Point ScrollOffset { get; }
        RowColRange VisibleRowColRange { get; }

        Rectangle? GetRangeSelectBound();
    }
}
