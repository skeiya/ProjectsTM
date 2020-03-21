using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.gridControl1.OnDrawCell += GridControl1_OnDrawCell;
        }

        private void GridControl1_OnDrawCell(object sender, FreeGridControl.DrawCellEventArgs e)
        {
            if(e.RowIndex == 2 && e.ColIndex == 3)
            {
                int a = 0;
                a++;
            }
            var str = e.RowIndex.ToString() + "," + e.ColIndex.ToString();
            e.Graphics.DrawString(str, this.Font, Brushes.Red, e.Rect);
        }
    }
}
