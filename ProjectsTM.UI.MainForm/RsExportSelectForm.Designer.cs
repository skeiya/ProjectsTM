namespace ProjectsTM.UI.MainForm
{
    partial class RsExportSelectForm
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
            this.radioAll = new System.Windows.Forms.RadioButton();
            this.radioSelect = new System.Windows.Forms.RadioButton();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.textSelectGetsudo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // radioAll
            // 
            this.radioAll.AutoSize = true;
            this.radioAll.Checked = true;
            this.radioAll.Location = new System.Drawing.Point(25, 13);
            this.radioAll.Name = "radioAll";
            this.radioAll.Size = new System.Drawing.Size(59, 16);
            this.radioAll.TabIndex = 0;
            this.radioAll.TabStop = true;
            this.radioAll.Text = "全期間";
            this.radioAll.UseVisualStyleBackColor = true;
            // 
            // radioSelect
            // 
            this.radioSelect.AutoSize = true;
            this.radioSelect.Location = new System.Drawing.Point(25, 38);
            this.radioSelect.Name = "radioSelect";
            this.radioSelect.Size = new System.Drawing.Size(71, 16);
            this.radioSelect.TabIndex = 1;
            this.radioSelect.Text = "月度指定";
            this.radioSelect.UseVisualStyleBackColor = true;
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(139, 72);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 2;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(237, 72);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 3;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // textSelectGetsudo
            // 
            this.textSelectGetsudo.Location = new System.Drawing.Point(139, 38);
            this.textSelectGetsudo.Name = "textSelectGetsudo";
            this.textSelectGetsudo.Size = new System.Drawing.Size(173, 19);
            this.textSelectGetsudo.TabIndex = 4;
            this.textSelectGetsudo.Text = "2020/04";
            // 
            // RsExportSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 115);
            this.Controls.Add(this.textSelectGetsudo);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.radioSelect);
            this.Controls.Add(this.radioAll);
            this.Name = "RsExportSelectForm";
            this.Text = "EditRsSelectForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioAll;
        private System.Windows.Forms.RadioButton radioSelect;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.TextBox textSelectGetsudo;
    }
}