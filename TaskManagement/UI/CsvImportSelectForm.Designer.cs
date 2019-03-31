namespace TaskManagement
{
    partial class CsvImportSelectForm
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
            this.radioButtonWorkingDays = new System.Windows.Forms.RadioButton();
            this.radioButtonMembers = new System.Windows.Forms.RadioButton();
            this.radioButtonWorkItems = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // radioButtonWorkingDays
            // 
            this.radioButtonWorkingDays.AutoSize = true;
            this.radioButtonWorkingDays.Checked = true;
            this.radioButtonWorkingDays.Location = new System.Drawing.Point(37, 32);
            this.radioButtonWorkingDays.Name = "radioButtonWorkingDays";
            this.radioButtonWorkingDays.Size = new System.Drawing.Size(161, 28);
            this.radioButtonWorkingDays.TabIndex = 0;
            this.radioButtonWorkingDays.TabStop = true;
            this.radioButtonWorkingDays.Text = "稼働日情報";
            this.radioButtonWorkingDays.UseVisualStyleBackColor = true;
            // 
            // radioButtonMembers
            // 
            this.radioButtonMembers.AutoSize = true;
            this.radioButtonMembers.Location = new System.Drawing.Point(37, 67);
            this.radioButtonMembers.Name = "radioButtonMembers";
            this.radioButtonMembers.Size = new System.Drawing.Size(164, 28);
            this.radioButtonMembers.TabIndex = 1;
            this.radioButtonMembers.Text = "メンバー情報";
            this.radioButtonMembers.UseVisualStyleBackColor = true;
            // 
            // radioButtonWorkItems
            // 
            this.radioButtonWorkItems.AutoSize = true;
            this.radioButtonWorkItems.Location = new System.Drawing.Point(37, 102);
            this.radioButtonWorkItems.Name = "radioButtonWorkItems";
            this.radioButtonWorkItems.Size = new System.Drawing.Size(185, 28);
            this.radioButtonWorkItems.TabIndex = 2;
            this.radioButtonWorkItems.Text = "作業項目情報";
            this.radioButtonWorkItems.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(196, 179);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(83, 43);
            this.button1.TabIndex = 3;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(297, 179);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(148, 43);
            this.button2.TabIndex = 3;
            this.button2.Text = "キャンセル";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // CsvImportSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 234);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.radioButtonWorkItems);
            this.Controls.Add(this.radioButtonMembers);
            this.Controls.Add(this.radioButtonWorkingDays);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "CsvImportSelectForm";
            this.Text = "CSVインポート";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonWorkingDays;
        private System.Windows.Forms.RadioButton radioButtonMembers;
        private System.Windows.Forms.RadioButton radioButtonWorkItems;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}