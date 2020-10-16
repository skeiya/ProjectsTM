namespace ProjectsTM.UI.MainForm
{
    partial class FilterForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxFrom = new System.Windows.Forms.TextBox();
            this.textBoxTo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonClearWorkItem = new System.Windows.Forms.Button();
            this.buttonClearPeriod = new System.Windows.Forms.Button();
            this.buttonClearMembers = new System.Windows.Forms.Button();
            this.buttonImport = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.checkBoxSort = new System.Windows.Forms.CheckBox();
            this.buttonAllOff = new System.Windows.Forms.Button();
            this.buttonGenerateFromProject = new System.Windows.Forms.Button();
            this.buttonGenerateFromWorkItems = new System.Windows.Forms.Button();
            this.buttonFromTodayToSpecialDay = new System.Windows.Forms.Button();
            this.checkBox_IsFreeTimeMemberShow = new System.Windows.Forms.CheckBox();
            this.comboBoxPattern = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox_MSFiltersSearchPattern = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "日付の範囲";
            // 
            // textBoxFrom
            // 
            this.textBoxFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFrom.Location = new System.Drawing.Point(37, 127);
            this.textBoxFrom.Name = "textBoxFrom";
            this.textBoxFrom.Size = new System.Drawing.Size(626, 25);
            this.textBoxFrom.TabIndex = 4;
            // 
            // textBoxTo
            // 
            this.textBoxTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTo.Location = new System.Drawing.Point(37, 178);
            this.textBoxTo.Name = "textBoxTo";
            this.textBoxTo.Size = new System.Drawing.Size(859, 25);
            this.textBoxTo.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 235);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 18);
            this.label2.TabIndex = 8;
            this.label2.Text = "メンバーの選択";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(15, 273);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(957, 224);
            this.checkedListBox1.TabIndex = 10;
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApply.Location = new System.Drawing.Point(760, 636);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(103, 34);
            this.buttonApply.TabIndex = 11;
            this.buttonApply.Text = "OK";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(868, 636);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(103, 34);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "閉じる";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "作業項目";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(92, 157);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 18);
            this.label4.TabIndex = 5;
            this.label4.Text = "から";
            // 
            // buttonClearWorkItem
            // 
            this.buttonClearWorkItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearWorkItem.Location = new System.Drawing.Point(905, 36);
            this.buttonClearWorkItem.Name = "buttonClearWorkItem";
            this.buttonClearWorkItem.Size = new System.Drawing.Size(65, 33);
            this.buttonClearWorkItem.TabIndex = 2;
            this.buttonClearWorkItem.Text = "クリア";
            this.buttonClearWorkItem.UseVisualStyleBackColor = true;
            this.buttonClearWorkItem.Click += new System.EventHandler(this.buttonClearWorkItem_Click);
            // 
            // buttonClearPeriod
            // 
            this.buttonClearPeriod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearPeriod.Location = new System.Drawing.Point(907, 124);
            this.buttonClearPeriod.Name = "buttonClearPeriod";
            this.buttonClearPeriod.Size = new System.Drawing.Size(65, 33);
            this.buttonClearPeriod.TabIndex = 7;
            this.buttonClearPeriod.Text = "クリア";
            this.buttonClearPeriod.UseVisualStyleBackColor = true;
            this.buttonClearPeriod.Click += new System.EventHandler(this.buttonClearPeriod_Click);
            // 
            // buttonClearMembers
            // 
            this.buttonClearMembers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearMembers.Location = new System.Drawing.Point(905, 229);
            this.buttonClearMembers.Name = "buttonClearMembers";
            this.buttonClearMembers.Size = new System.Drawing.Size(65, 30);
            this.buttonClearMembers.TabIndex = 9;
            this.buttonClearMembers.Text = "クリア";
            this.buttonClearMembers.UseVisualStyleBackColor = true;
            this.buttonClearMembers.Click += new System.EventHandler(this.buttonClearMembers_Click);
            // 
            // buttonImport
            // 
            this.buttonImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonImport.Location = new System.Drawing.Point(688, 598);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(142, 30);
            this.buttonImport.TabIndex = 9;
            this.buttonImport.Text = "インポート...";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExport.Location = new System.Drawing.Point(833, 600);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(137, 30);
            this.buttonExport.TabIndex = 9;
            this.buttonExport.Text = "エクスポート...";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // checkBoxSort
            // 
            this.checkBoxSort.AutoSize = true;
            this.checkBoxSort.Location = new System.Drawing.Point(608, 234);
            this.checkBoxSort.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.checkBoxSort.Name = "checkBoxSort";
            this.checkBoxSort.Size = new System.Drawing.Size(127, 22);
            this.checkBoxSort.TabIndex = 13;
            this.checkBoxSort.Text = "所属でソート";
            this.checkBoxSort.UseVisualStyleBackColor = true;
            this.checkBoxSort.CheckedChanged += new System.EventHandler(this.CheckBoxSort_CheckedChanged);
            // 
            // buttonAllOff
            // 
            this.buttonAllOff.Location = new System.Drawing.Point(757, 230);
            this.buttonAllOff.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonAllOff.Name = "buttonAllOff";
            this.buttonAllOff.Size = new System.Drawing.Size(103, 30);
            this.buttonAllOff.TabIndex = 14;
            this.buttonAllOff.Text = "全OFF";
            this.buttonAllOff.UseVisualStyleBackColor = true;
            this.buttonAllOff.Click += new System.EventHandler(this.ButtonAllOff_Click);
            // 
            // buttonGenerateFromProject
            // 
            this.buttonGenerateFromProject.Location = new System.Drawing.Point(145, 228);
            this.buttonGenerateFromProject.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonGenerateFromProject.Name = "buttonGenerateFromProject";
            this.buttonGenerateFromProject.Size = new System.Drawing.Size(210, 34);
            this.buttonGenerateFromProject.TabIndex = 15;
            this.buttonGenerateFromProject.Text = "プロジェクトから生成...";
            this.buttonGenerateFromProject.UseVisualStyleBackColor = true;
            this.buttonGenerateFromProject.Click += new System.EventHandler(this.buttonGenerateFromProject_Click);
            // 
            // buttonGenerateFromWorkItems
            // 
            this.buttonGenerateFromWorkItems.Location = new System.Drawing.Point(365, 228);
            this.buttonGenerateFromWorkItems.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonGenerateFromWorkItems.Name = "buttonGenerateFromWorkItems";
            this.buttonGenerateFromWorkItems.Size = new System.Drawing.Size(233, 34);
            this.buttonGenerateFromWorkItems.TabIndex = 16;
            this.buttonGenerateFromWorkItems.Text = "作業項目マッチから生成...";
            this.buttonGenerateFromWorkItems.UseVisualStyleBackColor = true;
            this.buttonGenerateFromWorkItems.Click += new System.EventHandler(this.buttonGenerateFromWorkItems_Click);
            // 
            // buttonFromTodayToSpecialDay
            // 
            this.buttonFromTodayToSpecialDay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFromTodayToSpecialDay.Location = new System.Drawing.Point(673, 124);
            this.buttonFromTodayToSpecialDay.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonFromTodayToSpecialDay.Name = "buttonFromTodayToSpecialDay";
            this.buttonFromTodayToSpecialDay.Size = new System.Drawing.Size(225, 34);
            this.buttonFromTodayToSpecialDay.TabIndex = 17;
            this.buttonFromTodayToSpecialDay.Text = "今日から";
            this.buttonFromTodayToSpecialDay.UseVisualStyleBackColor = true;
            this.buttonFromTodayToSpecialDay.Click += new System.EventHandler(this.buttonFromTodayToSpecialDay_Click);
            // 
            // checkBox_IsFreeTimeMemberShow
            // 
            this.checkBox_IsFreeTimeMemberShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_IsFreeTimeMemberShow.AutoSize = true;
            this.checkBox_IsFreeTimeMemberShow.Location = new System.Drawing.Point(37, 520);
            this.checkBox_IsFreeTimeMemberShow.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.checkBox_IsFreeTimeMemberShow.Name = "checkBox_IsFreeTimeMemberShow";
            this.checkBox_IsFreeTimeMemberShow.Size = new System.Drawing.Size(251, 22);
            this.checkBox_IsFreeTimeMemberShow.TabIndex = 18;
            this.checkBox_IsFreeTimeMemberShow.Text = "タスクがないメンバーを表示する";
            this.checkBox_IsFreeTimeMemberShow.UseVisualStyleBackColor = true;
            // 
            // comboBoxPattern
            // 
            this.comboBoxPattern.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPattern.FormattingEnabled = true;
            this.comboBoxPattern.Location = new System.Drawing.Point(37, 43);
            this.comboBoxPattern.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.comboBoxPattern.Name = "comboBoxPattern";
            this.comboBoxPattern.Size = new System.Drawing.Size(859, 26);
            this.comboBoxPattern.TabIndex = 19;
            this.comboBoxPattern.DropDown += new System.EventHandler(this.comboBoxPattern_DropDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 560);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(211, 18);
            this.label5.TabIndex = 21;
            this.label5.Text = "マイルストンフィルタ検索設定";
            // 
            // comboBox_MSFiltersSearchPattern
            // 
            this.comboBox_MSFiltersSearchPattern.FormattingEnabled = true;
            this.comboBox_MSFiltersSearchPattern.Location = new System.Drawing.Point(252, 555);
            this.comboBox_MSFiltersSearchPattern.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.comboBox_MSFiltersSearchPattern.Name = "comboBox_MSFiltersSearchPattern";
            this.comboBox_MSFiltersSearchPattern.Size = new System.Drawing.Size(199, 26);
            this.comboBox_MSFiltersSearchPattern.TabIndex = 22;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(462, 560);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(186, 18);
            this.label6.TabIndex = 24;
            this.label6.Text = "(\"|\"区切り, 正規表現可)";
            // 
            // FilterForm
            // 
            this.AcceptButton = this.buttonApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(980, 681);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBox_MSFiltersSearchPattern);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxPattern);
            this.Controls.Add(this.checkBox_IsFreeTimeMemberShow);
            this.Controls.Add(this.buttonFromTodayToSpecialDay);
            this.Controls.Add(this.buttonGenerateFromWorkItems);
            this.Controls.Add(this.buttonGenerateFromProject);
            this.Controls.Add(this.buttonAllOff);
            this.Controls.Add(this.checkBoxSort);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.buttonImport);
            this.Controls.Add(this.buttonClearMembers);
            this.Controls.Add(this.buttonClearPeriod);
            this.Controls.Add(this.buttonClearWorkItem);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxTo);
            this.Controls.Add(this.textBoxFrom);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FilterForm";
            this.ShowIcon = false;
            this.Text = "フィルター";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxFrom;
        private System.Windows.Forms.TextBox textBoxTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonClearWorkItem;
        private System.Windows.Forms.Button buttonClearPeriod;
        private System.Windows.Forms.Button buttonClearMembers;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.CheckBox checkBoxSort;
        private System.Windows.Forms.Button buttonAllOff;
        private System.Windows.Forms.Button buttonGenerateFromProject;
        private System.Windows.Forms.Button buttonGenerateFromWorkItems;
        private System.Windows.Forms.Button buttonFromTodayToSpecialDay;
        private System.Windows.Forms.CheckBox checkBox_IsFreeTimeMemberShow;
        private System.Windows.Forms.ComboBox comboBoxPattern;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox_MSFiltersSearchPattern;
        private System.Windows.Forms.Label label6;
    }
}