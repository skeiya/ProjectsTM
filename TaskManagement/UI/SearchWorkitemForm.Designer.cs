namespace TaskManagement.UI
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
            this.components = new System.ComponentModel.Container();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.textBoxPattern = new System.Windows.Forms.TextBox();
            this.checkBoxOverwrapPeriod = new System.Windows.Forms.CheckBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.searchWorkitemFormBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.searchWorkitemFormBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(10, 33);
            this.listBox1.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(414, 184);
            this.listBox1.TabIndex = 3;
            this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListBox1_MouseDoubleClick);
            // 
            // textBoxPattern
            // 
            this.textBoxPattern.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPattern.Location = new System.Drawing.Point(10, 8);
            this.textBoxPattern.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.textBoxPattern.Name = "textBoxPattern";
            this.textBoxPattern.Size = new System.Drawing.Size(264, 19);
            this.textBoxPattern.TabIndex = 0;
            // 
            // checkBoxOverwrapPeriod
            // 
            this.checkBoxOverwrapPeriod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxOverwrapPeriod.AutoSize = true;
            this.checkBoxOverwrapPeriod.Location = new System.Drawing.Point(286, 10);
            this.checkBoxOverwrapPeriod.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.checkBoxOverwrapPeriod.Name = "checkBoxOverwrapPeriod";
            this.checkBoxOverwrapPeriod.Size = new System.Drawing.Size(72, 16);
            this.checkBoxOverwrapPeriod.TabIndex = 1;
            this.checkBoxOverwrapPeriod.Text = "期間重複";
            this.checkBoxOverwrapPeriod.UseVisualStyleBackColor = true;
            this.checkBoxOverwrapPeriod.CheckedChanged += new System.EventHandler(this.CheckBoxOverwrapPeriod_CheckedChanged);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.Location = new System.Drawing.Point(377, 6);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(47, 23);
            this.buttonSearch.TabIndex = 2;
            this.buttonSearch.Text = "検索";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.ButtonSearch_Click);
            // 
            // searchWorkitemFormBindingSource
            // 
            this.searchWorkitemFormBindingSource.DataSource = typeof(TaskManagement.UI.SearchWorkitemForm);
            // 
            // SearchWorkitemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 230);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.checkBoxOverwrapPeriod);
            this.Controls.Add(this.textBoxPattern);
            this.Controls.Add(this.listBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Name = "SearchWorkitemForm";
            this.Text = "検索";
            ((System.ComponentModel.ISupportInitialize)(this.searchWorkitemFormBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox textBoxPattern;
        private System.Windows.Forms.CheckBox checkBoxOverwrapPeriod;
        private System.Windows.Forms.BindingSource searchWorkitemFormBindingSource;
        private System.Windows.Forms.Button buttonSearch;
    }
}