namespace TaskManagement
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxWorkItem = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonClearWorkItem = new System.Windows.Forms.Button();
            this.buttonClearPeriod = new System.Windows.Forms.Button();
            this.buttonClearMembers = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "日付の範囲";
            // 
            // textBoxFrom
            // 
            this.textBoxFrom.Location = new System.Drawing.Point(48, 149);
            this.textBoxFrom.Name = "textBoxFrom";
            this.textBoxFrom.Size = new System.Drawing.Size(200, 31);
            this.textBoxFrom.TabIndex = 1;
            // 
            // textBoxTo
            // 
            this.textBoxTo.Location = new System.Drawing.Point(48, 216);
            this.textBoxTo.Name = "textBoxTo";
            this.textBoxTo.Size = new System.Drawing.Size(200, 31);
            this.textBoxTo.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 269);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "メンバーの選択";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(17, 311);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(368, 498);
            this.checkedListBox1.TabIndex = 3;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(113, 829);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(133, 46);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "適用";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(252, 829);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(133, 46);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "閉じる";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 24);
            this.label3.TabIndex = 0;
            this.label3.Text = "作業項目";
            // 
            // textBoxWorkItem
            // 
            this.textBoxWorkItem.Location = new System.Drawing.Point(48, 53);
            this.textBoxWorkItem.Name = "textBoxWorkItem";
            this.textBoxWorkItem.Size = new System.Drawing.Size(200, 31);
            this.textBoxWorkItem.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(118, 187);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 24);
            this.label4.TabIndex = 0;
            this.label4.Text = "から";
            // 
            // buttonClearWorkItem
            // 
            this.buttonClearWorkItem.Location = new System.Drawing.Point(300, 53);
            this.buttonClearWorkItem.Name = "buttonClearWorkItem";
            this.buttonClearWorkItem.Size = new System.Drawing.Size(85, 31);
            this.buttonClearWorkItem.TabIndex = 5;
            this.buttonClearWorkItem.Text = "クリア";
            this.buttonClearWorkItem.UseVisualStyleBackColor = true;
            this.buttonClearWorkItem.Click += new System.EventHandler(this.buttonClearWorkItem_Click);
            // 
            // buttonClearPeriod
            // 
            this.buttonClearPeriod.Location = new System.Drawing.Point(300, 149);
            this.buttonClearPeriod.Name = "buttonClearPeriod";
            this.buttonClearPeriod.Size = new System.Drawing.Size(85, 31);
            this.buttonClearPeriod.TabIndex = 5;
            this.buttonClearPeriod.Text = "クリア";
            this.buttonClearPeriod.UseVisualStyleBackColor = true;
            this.buttonClearPeriod.Click += new System.EventHandler(this.buttonClearPeriod_Click);
            // 
            // buttonClearMembers
            // 
            this.buttonClearMembers.Location = new System.Drawing.Point(300, 269);
            this.buttonClearMembers.Name = "buttonClearMembers";
            this.buttonClearMembers.Size = new System.Drawing.Size(85, 31);
            this.buttonClearMembers.TabIndex = 5;
            this.buttonClearMembers.Text = "クリア";
            this.buttonClearMembers.UseVisualStyleBackColor = true;
            this.buttonClearMembers.Click += new System.EventHandler(this.buttonClearMembers_Click);
            // 
            // FilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 887);
            this.Controls.Add(this.buttonClearMembers);
            this.Controls.Add(this.buttonClearPeriod);
            this.Controls.Add(this.buttonClearWorkItem);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxTo);
            this.Controls.Add(this.textBoxWorkItem);
            this.Controls.Add(this.textBoxFrom);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FilterForm";
            this.ShowIcon = false;
            this.Text = "FilterForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxFrom;
        private System.Windows.Forms.TextBox textBoxTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxWorkItem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonClearWorkItem;
        private System.Windows.Forms.Button buttonClearPeriod;
        private System.Windows.Forms.Button buttonClearMembers;
    }
}