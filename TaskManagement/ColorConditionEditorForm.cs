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
    public partial class ColorConditionEditorForm : Form
    {
        public ColorConditionEditorForm()
        {
            InitializeComponent();
        }

        public ColorCondition ColorCondition { get; internal set; }

        private void button1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.OK) return;
            SetColor(colorDialog1.Color);
        }

        private void SetColor(Color color)
        {
            textBox1.BackColor = color;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ColorCondition = new ColorCondition(textBox1.Text, colorDialog1.Color);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
