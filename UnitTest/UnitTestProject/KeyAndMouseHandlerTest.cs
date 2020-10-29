using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectsTM.Model;
using ProjectsTM.Service;
using ProjectsTM.UI.MainForm;
using ProjectsTM.ViewModel;

namespace UnitTestProject
{
    [TestClass]
    public class KeyAndMouseHandlerTest
    {
        [TestMethod]
        public void MouseLeftDown_and_WorkItemSelect()
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
            var i = new WorkItem(
                new Project(""), "", Tags.Parse(""),
                new Period(new CallenderDay(2018, 4, 1), new CallenderDay(2018, 5, 2)),
                ichiro, TaskState.Active, "");
            var g = new WorkItem(
                new Project(""), "", Tags.Parse(""),
                new Period(new CallenderDay(2018, 6, 3), new CallenderDay(2018, 8, 5)),
                gozzila, TaskState.Active, "");
            appData.WorkItems.Add(i);
            appData.WorkItems.Add(g);

            var viewData = new ViewData(appData, new UndoService());
            var grid = new WorkItemGrid();
            grid.Initialize(viewData);
            var dragService = new WorkItemDragService();
            var drawService = new DrawService();
            drawService.Initialize(viewData, grid, dragService.IsActive, new Font(FontFamily.GenericSansSerif, 8));
            var editService = new WorkItemEditService(viewData);
            var service = new KeyAndMouseHandleService(viewData, grid, dragService, drawService, editService, grid);

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
    }
}
