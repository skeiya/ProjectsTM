namespace ProjectsTM.UI.Main
{
    partial class ManagementWokingDaysForm
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonImport = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.numericUpDownSeq = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxContainsWeekEnd = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSeq)).BeginInit();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(6, 6);
            this.listView1.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(289, 214);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "稼働日";
            this.columnHeader1.Width = 590;
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDelete.Location = new System.Drawing.Point(297, 24);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(77, 24);
            this.buttonDelete.TabIndex = 1;
            this.buttonDelete.Text = "削除(&D)";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.ButtonDelete_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdd.Location = new System.Drawing.Point(297, 196);
            this.buttonAdd.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(77, 24);
            this.buttonAdd.TabIndex = 7;
            this.buttonAdd.Text = "追加(&A)";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.ButtonAdd_Click);
            // 
            // buttonImport
            // 
            this.buttonImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonImport.Location = new System.Drawing.Point(297, 63);
            this.buttonImport.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(77, 24);
            this.buttonImport.TabIndex = 2;
            this.buttonImport.Text = "インポート(&I)";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.ButtonImport_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy/MM/dd";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(6, 226);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(135, 19);
            this.dateTimePicker1.TabIndex = 3;
            // 
            // numericUpDownSeq
            // 
            this.numericUpDownSeq.Location = new System.Drawing.Point(212, 226);
            this.numericUpDownSeq.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownSeq.Name = "numericUpDownSeq";
            this.numericUpDownSeq.Size = new System.Drawing.Size(80, 19);
            this.numericUpDownSeq.TabIndex = 5;
            this.numericUpDownSeq.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(153, 231);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "連続日数";
            // 
            // checkBoxContainsWeekEnd
            // 
            this.checkBoxContainsWeekEnd.AutoSize = true;
            this.checkBoxContainsWeekEnd.Location = new System.Drawing.Point(297, 227);
            this.checkBoxContainsWeekEnd.Name = "checkBoxContainsWeekEnd";
            this.checkBoxContainsWeekEnd.Size = new System.Drawing.Size(70, 16);
            this.checkBoxContainsWeekEnd.TabIndex = 6;
            this.checkBoxContainsWeekEnd.Text = "土日含む";
            this.checkBoxContainsWeekEnd.UseVisualStyleBackColor = true;
            // 
            // ManagementWokingDaysForm
            // 
            this.AcceptButton = this.buttonAdd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 253);
            this.Controls.Add(this.checkBoxContainsWeekEnd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownSeq);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.buttonImport);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.listView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Name = "ManagementWokingDaysForm";
            this.Text = "稼働日管理";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSeq)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.NumericUpDown numericUpDownSeq;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxContainsWeekEnd;
    }
}