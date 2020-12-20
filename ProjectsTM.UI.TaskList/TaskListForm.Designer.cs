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
            this.buttonShowMyTaskList = new System.Windows.Forms.Button();
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
            this.gridControl1.Location = new System.Drawing.Point(13, 58);
            this.gridControl1.LockUpdate = true;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1214, 355);
            this.gridControl1.TabIndex = 4;
            this.gridControl1.VOffset = 0;
            // 
            // comboBoxPattern
            // 
            this.comboBoxPattern.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPattern.FormattingEnabled = true;
            this.comboBoxPattern.Location = new System.Drawing.Point(13, 18);
            this.comboBoxPattern.Name = "comboBoxPattern";
            this.comboBoxPattern.Size = new System.Drawing.Size(655, 26);
            this.comboBoxPattern.TabIndex = 0;
            this.comboBoxPattern.DropDown += new System.EventHandler(this.comboBoxPattern_DropDown);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpdate.Location = new System.Drawing.Point(1105, 17);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(125, 34);
            this.buttonUpdate.TabIndex = 3;
            this.buttonUpdate.Text = "更新";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // labelSum
            // 
            this.labelSum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSum.AutoSize = true;
            this.labelSum.Location = new System.Drawing.Point(20, 432);
            this.labelSum.Name = "labelSum";
            this.labelSum.Size = new System.Drawing.Size(44, 18);
            this.labelSum.TabIndex = 5;
            this.labelSum.Text = "合計";
            // 
            // labelErrorCount
            // 
            this.labelErrorCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelErrorCount.AutoSize = true;
            this.labelErrorCount.Location = new System.Drawing.Point(247, 432);
            this.labelErrorCount.Name = "labelErrorCount";
            this.labelErrorCount.Size = new System.Drawing.Size(67, 18);
            this.labelErrorCount.TabIndex = 6;
            this.labelErrorCount.Text = "エラー数";
            // 
            // checkBoxShowMS
            // 
            this.checkBoxShowMS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxShowMS.AutoSize = true;
            this.checkBoxShowMS.Checked = true;
            this.checkBoxShowMS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowMS.Location = new System.Drawing.Point(695, 23);
            this.checkBoxShowMS.Name = "checkBoxShowMS";
            this.checkBoxShowMS.Size = new System.Drawing.Size(94, 22);
            this.checkBoxShowMS.TabIndex = 1;
            this.checkBoxShowMS.Text = "MS表示";
            this.checkBoxShowMS.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(448, 432);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 18);
            this.label1.TabIndex = 7;
            this.label1.Text = "&&";
            // 
            // textBoxAndCondition
            // 
            this.textBoxAndCondition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAndCondition.Location = new System.Drawing.Point(478, 428);
            this.textBoxAndCondition.Name = "textBoxAndCondition";
            this.textBoxAndCondition.Size = new System.Drawing.Size(749, 25);
            this.textBoxAndCondition.TabIndex = 8;
            // 
            // buttonEazyRegex
            // 
            this.buttonEazyRegex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEazyRegex.Location = new System.Drawing.Point(801, 16);
            this.buttonEazyRegex.Name = "buttonEazyRegex";
            this.buttonEazyRegex.Size = new System.Drawing.Size(125, 34);
            this.buttonEazyRegex.TabIndex = 2;
            this.buttonEazyRegex.Text = "簡易指定...";
            this.buttonEazyRegex.UseVisualStyleBackColor = true;
            // 
            // buttonShowMyTaskList
            // 
            this.buttonShowMyTaskList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonShowMyTaskList.Location = new System.Drawing.Point(936, 16);
            this.buttonShowMyTaskList.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonShowMyTaskList.Name = "buttonShowMyTaskList";
            this.buttonShowMyTaskList.Size = new System.Drawing.Size(161, 34);
            this.buttonShowMyTaskList.TabIndex = 9;
            this.buttonShowMyTaskList.Text = "自己タスク一覧";
            this.buttonShowMyTaskList.UseVisualStyleBackColor = true;
            this.buttonShowMyTaskList.Click += new System.EventHandler(this.button1_Click);
            // 
            // TaskListForm
            // 
            this.AcceptButton = this.buttonUpdate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1250, 460);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.buttonShowMyTaskList);
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
        private System.Windows.Forms.Button buttonShowMyTaskList;
        private TaskListGrid gridControl1;
    }
}