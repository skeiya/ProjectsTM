using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManagement
{
    public partial class Form1 : Form
    {
        private AppData _appData;

        public Form1()
        {
            InitializeComponent();
            printDocument1.PrintPage += PrintDocument1_PrintPage;
            _appData = new AppData();
        }

        private void PrintDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            var bounds = e.PageBounds;
            e.Graphics.FillRectangle(Brushes.Red, bounds);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            printDialog1.Document = printDocument1;
            if (printDialog1.ShowDialog() != DialogResult.OK) return;
            printDocument1.Print();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            if (printPreviewDialog1.ShowDialog() != DialogResult.OK) return;
            
        }
    }
}
