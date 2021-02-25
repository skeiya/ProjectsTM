using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectsTM.Model;
using ProjectsTM.Service;
using ProjectsTM.UI.Main;
using ProjectsTM.ViewModel;
using System.Drawing;
using System.Windows.Forms;

namespace UnitTestProject
{
    [TestClass]
    public class KeyAndMouseHandlerTest
    {
        private static void PrepareCommon(out WorkItem i, out WorkItem g, out MainViewData viewData, out KeyAndMouseHandleService service, out WorkItemGrid grid)
        {
            var appData = new AppData();
            var ichiro = new Member("鈴木", "イチロー", "マリナーズ");
            var gozzila = new Member("松井", "秀喜", "ヤンキース");
            appData.Members.Add(ichiro);
            appData.Members.Add(gozzila);
            appData.Callender.Add(new CallenderDay(2018, 4, 1));
            appData.Callender.Add(new CallenderDay(2018, 5, 2));
            appData.Callender.Add(new CallenderDay(2018, 6, 3));
            appData.Callender.Add(new CallenderDay(2018, 7, 4));
            appData.Callender.Add(new CallenderDay(2018, 8, 5));
            i = new WorkItem(
                new Project(""), "", Tags.Parse(""),
                new Period(new CallenderDay(2018, 4, 1), new CallenderDay(2018, 5, 2)),
                ichiro, TaskState.Active, "");
            g = new WorkItem(
                new Project(""), "", Tags.Parse(""),
                new Period(new CallenderDay(2018, 6, 3), new CallenderDay(2018, 8, 5)),
                gozzila, TaskState.Active, "");
            appData.WorkItems.Add(i);
            appData.WorkItems.Add(g);

            viewData = new MainViewData(appData);
            grid = new WorkItemGrid(viewData, new EditorFindService());

            /* グリッドの列幅・行高
              24 12 12 35 35
            9
            9
            9
            9           i
            9           i
            9             g
            9             g
            9             g
            */
            grid.ColWidths[0] = 24;
            grid.ColWidths[1] = 12;
            grid.ColWidths[2] = 12;
            grid.ColWidths[3] = 35;
            grid.ColWidths[4] = 35;
            grid.RowHeights[0] = 9;
            grid.RowHeights[1] = 9;
            grid.RowHeights[2] = 9;
            grid.RowHeights[3] = 9;
            grid.RowHeights[4] = 9;
            grid.RowHeights[5] = 9;
            grid.RowHeights[6] = 9;
            grid.RowHeights[7] = 9;

            var dragService = new WorkItemDragService();
            var drawService = new DrawService(viewData, grid, dragService.IsActive, dragService.IsMoveing, () => dragService.DragStartInfo, new Font(FontFamily.GenericSansSerif, 8));
            var editService = new WorkItemEditService(viewData.Core);
            service = new KeyAndMouseHandleService(viewData.Core, grid, dragService, drawService, editService, grid, new EditorFindService(), null);
        }

        [TestMethod]
        public void MouseLeftDown_and_WorkItemSelect()
        {
            PrepareCommon(out var i, out var g, out var viewData, out var service, out _);

            service.MouseDown(new MouseEventArgs(MouseButtons.Left, 1, 50, 30, 0));
            Assert.AreEqual(viewData.Selected.Unique, i);

            service.MouseDown(new MouseEventArgs(MouseButtons.Left, 1, 50, 39, 0));
            Assert.AreEqual(viewData.Selected.Unique, i);

            service.MouseDown(new MouseEventArgs(MouseButtons.Left, 1, 85, 48, 0));
            Assert.AreEqual(viewData.Selected.Unique, g);

            service.MouseDown(new MouseEventArgs(MouseButtons.Left, 1, 85, 57, 0));
            Assert.AreEqual(viewData.Selected.Unique, g);

            service.MouseDown(new MouseEventArgs(MouseButtons.Left, 1, 85, 66, 0));
            Assert.AreEqual(viewData.Selected.Unique, g);
        }


        [TestMethod]
        public void MouseLeftDown_and_WorkItemDrag()
        {
            PrepareCommon(out var i, out _, out var viewData, out var service, out var grid);

            service.MouseDown(new MouseEventArgs(MouseButtons.Left, 1, 50, 30, 0));
            service.MouseDown(new MouseEventArgs(MouseButtons.Left, 1, 50, 30, 0)); // @@@ ここは不要にしたい
            service.MouseMove(new FreeGridControl.ClientPoint(grid.Raw2Client(new FreeGridControl.RawPoint(50, 39))), grid);
            service.MouseUp();

            i.Period = new Period(new CallenderDay(2018, 5, 2), new CallenderDay(2018, 6, 3));
            Assert.AreEqual(viewData.Selected.Unique, i);
        }
    }
}
