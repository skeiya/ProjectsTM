using ProjectsTM.Model;
using ProjectsTM.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectsTM.UI
{
    public partial class TaskListForm : Form
    {
        private readonly ViewData _viewData;

        public TaskListForm(ViewData viewData)
        {
            InitializeComponent();

            this._viewData = viewData;
            gridControl1.Initialize(viewData);
        }
    }
}
