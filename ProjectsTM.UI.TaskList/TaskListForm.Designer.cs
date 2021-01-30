namespace ProjectsTM.UI.TaskList
{
    partial class TaskListForm
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
            this.gridControl1 = new ProjectsTM.UI.TaskList.TaskListGrid();
            this.comboBoxPattern = new System.Windows.Forms.ComboBox();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.labelSum = new System.Windows.Forms.Label();
            this.labelErrorCount = new System.Windows.Forms.Label();
            this.checkBoxShowMS = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxAndCondition = new System.Windows.Forms.TextBox();
            this.buttonEazyRegex = new System.Windows.Forms.Button();
            this.comboBoxErrorDisplay = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl1.FixedColCount = 0;
            this.gridControl1.FixedRowCount = 0;
            this.gridControl1.HOffset = 0;
            this.gridControl1.Location = new System.Drawing.Point(8, 39);
            this.gridControl1.LockUpdate = true;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(730, 240);
            this.gridControl1.TabIndex = 4;
            this.gridControl1.VOffset = 0;
            // 
            // comboBoxPattern
            // 
            this.comboBoxPattern.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPattern.FormattingEnabled = true;
            this.comboBoxPattern.Location = new System.Drawing.Point(8, 12);
            this.comboBoxPattern.Name = "comboBoxPattern";
            this.comboBoxPattern.Size = new System.Drawing.Size(400, 20);
            this.comboBoxPattern.TabIndex = 0;
            this.comboBoxPattern.DropDown += new System.EventHandler(this.comboBoxPattern_DropDown);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpdate.Location = new System.Drawing.Point(663, 10);
            this.buttonUpdate.Margin = new System.Windows.Forms.Padding(2);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdate.TabIndex = 3;
            this.buttonUpdate.Text = "更新";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // labelSum
            // 
            this.labelSum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSum.AutoSize = true;
            this.labelSum.Location = new System.Drawing.Point(12, 288);
            this.labelSum.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSum.Name = "labelSum";
            this.labelSum.Size = new System.Drawing.Size(29, 12);
            this.labelSum.TabIndex = 5;
            this.labelSum.Text = "合計";
            // 
            // labelErrorCount
            // 
            this.labelErrorCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelErrorCount.AutoSize = true;
            this.labelErrorCount.Location = new System.Drawing.Point(148, 288);
            this.labelErrorCount.Name = "labelErrorCount";
            this.labelErrorCount.Size = new System.Drawing.Size(44, 12);
            this.labelErrorCount.TabIndex = 6;
            this.labelErrorCount.Text = "エラー数";
            // 
            // checkBoxShowMS
            // 
            this.checkBoxShowMS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxShowMS.AutoSize = true;
            this.checkBoxShowMS.Checked = true;
            this.checkBoxShowMS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowMS.Location = new System.Drawing.Point(495, 15);
            this.checkBoxShowMS.Name = "checkBoxShowMS";
            this.checkBoxShowMS.Size = new System.Drawing.Size(64, 16);
            this.checkBoxShowMS.TabIndex = 1;
            this.checkBoxShowMS.Text = "MS表示";
            this.checkBoxShowMS.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(269, 288);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "&&";
            // 
            // textBoxAndCondition
            // 
            this.textBoxAndCondition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAndCondition.Location = new System.Drawing.Point(287, 285);
            this.textBoxAndCondition.Name = "textBoxAndCondition";
            this.textBoxAndCondition.Size = new System.Drawing.Size(451, 19);
            this.textBoxAndCondition.TabIndex = 8;
            // 
            // buttonEazyRegex
            // 
            this.buttonEazyRegex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEazyRegex.Location = new System.Drawing.Point(414, 10);
            this.buttonEazyRegex.Name = "buttonEazyRegex";
            this.buttonEazyRegex.Size = new System.Drawing.Size(75, 23);
            this.buttonEazyRegex.TabIndex = 2;
            this.buttonEazyRegex.Text = "簡易指定...";
            this.buttonEazyRegex.UseVisualStyleBackColor = true;
            // 
            // comboBoxErrorDisplay
            // 
            this.comboBoxErrorDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxErrorDisplay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxErrorDisplay.FormattingEnabled = true;
            this.comboBoxErrorDisplay.Location = new System.Drawing.Point(565, 12);
            this.comboBoxErrorDisplay.Name = "comboBoxErrorDisplay";
            this.comboBoxErrorDisplay.Size = new System.Drawing.Size(93, 20);
            this.comboBoxErrorDisplay.TabIndex = 9;
            // 
            // TaskListForm
            // 
            this.AcceptButton = this.buttonUpdate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 307);
            this.Controls.Add(this.comboBoxErrorDisplay);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.buttonEazyRegex);
            this.Controls.Add(this.textBoxAndCondition);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxShowMS);
            this.Controls.Add(this.labelErrorCount);
            this.Controls.Add(this.labelSum);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.comboBoxPattern);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "TaskListForm";
            this.Text = "タスクリスト";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBoxPattern;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Label labelSum;
        private System.Windows.Forms.Label labelErrorCount;
        private System.Windows.Forms.CheckBox checkBoxShowMS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxAndCondition;
        private System.Windows.Forms.Button buttonEazyRegex;
        private TaskListGrid gridControl1;
        private System.Windows.Forms.ComboBox comboBoxErrorDisplay;
    }
}