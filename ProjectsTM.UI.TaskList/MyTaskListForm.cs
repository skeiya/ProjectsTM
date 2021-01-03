using ProjectsTM.Model;
using ProjectsTM.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ProjectsTM.UI.TaskList
{
    public partial class MyTaskListForm : Form
    {
        private readonly List<WorkItem> _myWorkItems;
        public MyTaskListForm(ViewData viewData, string userName)
        {
            _myWorkItems = viewData.Original.WorkItems.Select(wi => wi).Where(wi => IsMyTask(wi, userName)).ToList();

            this.Text = $"{userName}に割り当てられたタスク";
            InitializeComponent();
            InitializeMyListViewSetting();
            SetMyTaskList();
        }
        private static bool IsMyTask(WorkItem wi, string userName)
        {
            return wi.AssignedMember.NaturalString.Equals(userName);
        }
        private void SetMyTaskList()
        {
            var myTasks = _myWorkItems.Select(x => MakeDispString(x)).ToList();

            myTasks.ForEach(s => { _myTasklistView.Items.Add(new ListViewItem(s)); });
        }

        public static string[] MakeDispString(WorkItem workItem)
        {
            var dispString = new string[4];
            dispString[0] = workItem.Name;
            dispString[1] = workItem.Project.ToString();
            dispString[2] = workItem.Period.To.Date;
            dispString[3] = workItem.State.ToString();
            return dispString;
        }

        private void InitializeMyListViewSetting()
        {
            _myTasklistView.View = View.Details;

            _myTasklistView.Resize += _myTasklistView_Resize;
        }

        private void _myTasklistView_Resize(object sender, EventArgs e)
        {
            // -2 -> Fillの幅になる。
            _myTasklistView.Columns[_myTasklistView.Columns.Count - 1].Width = -2; 
        }
    }
}
