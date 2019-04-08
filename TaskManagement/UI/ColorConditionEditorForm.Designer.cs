namespace TaskManagement
{
    partial class ColorConditionEditorForm
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonFront = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 12);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(319, 31);
            this.textBox1.TabIndex = 0;
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(367, 12);
            this.buttonBack.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(139, 38);
            this.buttonBack.TabIndex = 1;
            this.buttonBack.Text = "背景色";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(692, 13);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(91, 38);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(787, 12);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(132, 38);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "キャンセル";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.button3_Click);
            // 
            // buttonFront
            // 
            this.buttonFront.Location = new System.Drawing.Point(510, 13);
            this.buttonFront.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.buttonFront.Name = "buttonFront";
            this.buttonFront.Size = new System.Drawing.Size(139, 38);
            this.buttonFront.TabIndex = 1;
            this.buttonFront.Text = "文字色";
            this.buttonFront.UseVisualStyleBackColor = true;
            this.buttonFront.Click += new System.EventHandler(this.buttonFront_Click);
            // 
            // ColorConditionEditorForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(932, 62);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonFront);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.Name = "ColorConditionEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "色条件";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonFront;
    }
}