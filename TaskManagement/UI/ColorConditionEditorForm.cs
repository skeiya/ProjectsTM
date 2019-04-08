﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace TaskManagement
{
    public partial class ColorConditionEditorForm : Form
    {
        public ColorConditionEditorForm(ColorCondition init)
        {
            InitializeComponent();
            textBox1.Text = init.Pattern;
            textBox1.BackColor = init.BackColor;
            textBox1.ForeColor = init.ForeColor;
        }

        public ColorCondition ColorCondition { get; private set; }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.OK) return;
            SetBackColor(colorDialog1.Color);
        }

        private void buttonFront_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.OK) return;
            SetFrontColor(colorDialog1.Color);
        }

        private void SetFrontColor(Color color)
        {
            textBox1.ForeColor = color;
        }

        private void SetBackColor(Color color)
        {
            textBox1.BackColor = color;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ColorCondition = new ColorCondition(textBox1.Text, textBox1.BackColor, textBox1.ForeColor);
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
