namespace TaskManagement
{
    partial class SearchWorkitemForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.textBoxPattern = new System.Windows.Forms.TextBox();
            this.checkBoxOverwrapPeriod = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 24;
            this.listBox1.Location = new System.Drawing.Point(13, 90);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(775, 340);
            this.listBox1.TabIndex = 0;
            this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListBox1_MouseDoubleClick);
            // 
            // textBoxPattern
            // 
            this.textBoxPattern.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPattern.Location = new System.Drawing.Point(13, 13);
            this.textBoxPattern.Name = "textBoxPattern";
            this.textBoxPattern.Size = new System.Drawing.Size(775, 31);
            this.textBoxPattern.TabIndex = 1;
            this.textBoxPattern.TextChanged += new System.EventHandler(this.TextBoxPattern_TextChanged);
            // 
            // checkBoxOverwrapPeriod
            // 
            this.checkBoxOverwrapPeriod.AutoSize = true;
            this.checkBoxOverwrapPeriod.Location = new System.Drawing.Point(13, 51);
            this.checkBoxOverwrapPeriod.Name = "checkBoxOverwrapPeriod";
            this.checkBoxOverwrapPeriod.Size = new System.Drawing.Size(173, 28);
            this.checkBoxOverwrapPeriod.TabIndex = 2;
            this.checkBoxOverwrapPeriod.Text = "期間重複あり";
            this.checkBoxOverwrapPeriod.UseVisualStyleBackColor = true;
            this.checkBoxOverwrapPeriod.CheckedChanged += new System.EventHandler(this.CheckBoxOverwrapPeriod_CheckedChanged);
            // 
            // SearchWorkitemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.checkBoxOverwrapPeriod);
            this.Controls.Add(this.textBoxPattern);
            this.Controls.Add(this.listBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "SearchWorkitemForm";
            this.Text = "検索";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox textBoxPattern;
        private System.Windows.Forms.CheckBox checkBoxOverwrapPeriod;
    }
}