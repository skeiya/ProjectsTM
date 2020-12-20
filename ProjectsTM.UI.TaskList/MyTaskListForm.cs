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
            MakeDispStr();
        }
        private bool IsMyTask(WorkItem wi, string userName)
        {
            return wi.AssignedMember.NaturalString.Equals(userName);
        }

        private void MakeDispStr()
        {
            var myTasks = _myWorkItems.Select(x => x.StringForMyTaskList()).ToList();

            myTasks.ForEach(s => {_myTasklistView.Items.Add(new ListViewItem(s));});
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
