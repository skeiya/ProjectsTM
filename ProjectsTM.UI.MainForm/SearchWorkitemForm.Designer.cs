namespace ProjectsTM.UI.MainForm
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
            this.checkBoxOverwrapPeriod = new System.Windows.Forms.CheckBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelSum = new System.Windows.Forms.Label();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.buttonEasyRegex = new System.Windows.Forms.Button();
            this.checkBoxCaseDistinct = new System.Windows.Forms.CheckBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.checkBoxIncludeMilestone = new System.Windows.Forms.CheckBox();
            this.comboBoxPattern = new System.Windows.Forms.ComboBox();
            this.searchWorkitemFormBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchWorkitemFormBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBoxOverwrapPeriod
            // 
            this.checkBoxOverwrapPeriod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxOverwrapPeriod.AutoSize = true;
            this.checkBoxOverwrapPeriod.Location = new System.Drawing.Point(515, 10);
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
            this.buttonSearch.Location = new System.Drawing.Point(670, 6);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(47, 23);
            this.buttonSearch.TabIndex = 2;
            this.buttonSearch.Text = "検索";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.ButtonSearch_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 211);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "合計";
            // 
            // labelSum
            // 
            this.labelSum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSum.AutoSize = true;
            this.labelSum.Location = new System.Drawing.Point(45, 211);
            this.labelSum.Name = "labelSum";
            this.labelSum.Size = new System.Drawing.Size(0, 12);
            this.labelSum.TabIndex = 5;
            // 
            // buttonSelect
            // 
            this.buttonSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelect.Location = new System.Drawing.Point(604, 206);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(111, 23);
            this.buttonSelect.TabIndex = 6;
            this.buttonSelect.Text = "グリッド上で選択";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // buttonEasyRegex
            // 
            this.buttonEasyRegex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEasyRegex.Location = new System.Drawing.Point(358, 6);
            this.buttonEasyRegex.Name = "buttonEasyRegex";
            this.buttonEasyRegex.Size = new System.Drawing.Size(75, 23);
            this.buttonEasyRegex.TabIndex = 7;
            this.buttonEasyRegex.Text = "簡易指定...";
            this.buttonEasyRegex.UseVisualStyleBackColor = true;
            this.buttonEasyRegex.Click += new System.EventHandler(this.buttonEasyRegex_Click);
            // 
            // checkBoxCaseDistinct
            // 
            this.checkBoxCaseDistinct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxCaseDistinct.AutoSize = true;
            this.checkBoxCaseDistinct.Location = new System.Drawing.Point(439, 11);
            this.checkBoxCaseDistinct.Name = "checkBoxCaseDistinct";
            this.checkBoxCaseDistinct.Size = new System.Drawing.Size(72, 16);
            this.checkBoxCaseDistinct.TabIndex = 8;
            this.checkBoxCaseDistinct.Text = "大小区別";
            this.checkBoxCaseDistinct.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(10, 35);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(707, 170);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // checkBoxIncludeMilestone
            // 
            this.checkBoxIncludeMilestone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxIncludeMilestone.AutoSize = true;
            this.checkBoxIncludeMilestone.Location = new System.Drawing.Point(591, 10);
            this.checkBoxIncludeMilestone.Name = "checkBoxIncludeMilestone";
            this.checkBoxIncludeMilestone.Size = new System.Drawing.Size(62, 16);
            this.checkBoxIncludeMilestone.TabIndex = 9;
            this.checkBoxIncludeMilestone.Text = "MS含む";
            this.checkBoxIncludeMilestone.UseVisualStyleBackColor = true;
            this.checkBoxIncludeMilestone.CheckedChanged += new System.EventHandler(this.checkBoxIncludeMilestone_CheckedChanged);
            // 
            // comboBoxPattern
            // 
            this.comboBoxPattern.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPattern.FormattingEnabled = true;
            this.comboBoxPattern.Location = new System.Drawing.Point(10, 8);
            this.comboBoxPattern.Name = "comboBoxPattern";
            this.comboBoxPattern.Size = new System.Drawing.Size(342, 20);
            this.comboBoxPattern.TabIndex = 10;
            this.comboBoxPattern.DropDown += new System.EventHandler(this.comboBoxPattern_DropDown);
            // 
            // searchWorkitemFormBindingSource
            // 
            this.searchWorkitemFormBindingSource.DataSource = typeof(ProjectsTM.UI.MainForm.SearchWorkitemForm);
            // 
            // SearchWorkitemForm
            // 
            this.AcceptButton = this.buttonSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 230);
            this.Controls.Add(this.comboBoxPattern);
            this.Controls.Add(this.checkBoxIncludeMilestone);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.checkBoxCaseDistinct);
            this.Controls.Add(this.buttonEasyRegex);
            this.Controls.Add(this.buttonSelect);
            this.Controls.Add(this.labelSum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.checkBoxOverwrapPeriod);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.MinimumSize = new System.Drawing.Size(743, 269);
            this.Name = "SearchWorkitemForm";
            this.Text = "検索";
            this.Load += new System.EventHandler(this.SearchWorkitemForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchWorkitemFormBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox checkBoxOverwrapPeriod;
        private System.Windows.Forms.BindingSource searchWorkitemFormBindingSource;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelSum;
        private System.Windows.Forms.Button buttonSelect;
        private System.Windows.Forms.Button buttonEasyRegex;
        private System.Windows.Forms.CheckBox checkBoxCaseDistinct;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckBox checkBoxIncludeMilestone;
        private System.Windows.Forms.ComboBox comboBoxPattern;
    }
}