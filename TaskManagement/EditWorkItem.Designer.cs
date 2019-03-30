namespace TaskManagement
{
    partial class EditWorkItem
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxWorkItemName = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBoxTags = new System.Windows.Forms.TextBox();
            this.comboBoxProject = new System.Windows.Forms.ComboBox();
            this.comboBoxAssignedMemer = new System.Windows.Forms.ComboBox();
            this.textBoxFrom = new System.Windows.Forms.TextBox();
            this.textBoxTo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonProjectEdit = new System.Windows.Forms.Button();
            this.buttonEditMembers = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "プロジェクト";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "担当者";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 207);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "期間";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 24);
            this.label4.TabIndex = 3;
            this.label4.Text = "作業項目名";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 329);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 24);
            this.label5.TabIndex = 4;
            this.label5.Text = "タグ";
            // 
            // textBoxWorkItemName
            // 
            this.textBoxWorkItemName.Location = new System.Drawing.Point(223, 17);
            this.textBoxWorkItemName.Name = "textBoxWorkItemName";
            this.textBoxWorkItemName.Size = new System.Drawing.Size(329, 31);
            this.textBoxWorkItemName.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(318, 395);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 43);
            this.button1.TabIndex = 6;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(414, 395);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(138, 43);
            this.button2.TabIndex = 6;
            this.button2.Text = "キャンセル";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // textBoxTags
            // 
            this.textBoxTags.Location = new System.Drawing.Point(223, 326);
            this.textBoxTags.Name = "textBoxTags";
            this.textBoxTags.Size = new System.Drawing.Size(329, 31);
            this.textBoxTags.TabIndex = 7;
            // 
            // comboBoxProject
            // 
            this.comboBoxProject.FormattingEnabled = true;
            this.comboBoxProject.Location = new System.Drawing.Point(223, 74);
            this.comboBoxProject.Name = "comboBoxProject";
            this.comboBoxProject.Size = new System.Drawing.Size(234, 32);
            this.comboBoxProject.TabIndex = 8;
            // 
            // comboBoxAssignedMemer
            // 
            this.comboBoxAssignedMemer.FormattingEnabled = true;
            this.comboBoxAssignedMemer.Location = new System.Drawing.Point(223, 139);
            this.comboBoxAssignedMemer.Name = "comboBoxAssignedMemer";
            this.comboBoxAssignedMemer.Size = new System.Drawing.Size(234, 32);
            this.comboBoxAssignedMemer.TabIndex = 9;
            // 
            // textBoxFrom
            // 
            this.textBoxFrom.Location = new System.Drawing.Point(223, 204);
            this.textBoxFrom.Name = "textBoxFrom";
            this.textBoxFrom.Size = new System.Drawing.Size(329, 31);
            this.textBoxFrom.TabIndex = 7;
            // 
            // textBoxTo
            // 
            this.textBoxTo.Location = new System.Drawing.Point(223, 267);
            this.textBoxTo.Name = "textBoxTo";
            this.textBoxTo.Size = new System.Drawing.Size(329, 31);
            this.textBoxTo.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(356, 241);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 24);
            this.label6.TabIndex = 10;
            this.label6.Text = "から";
            // 
            // buttonProjectEdit
            // 
            this.buttonProjectEdit.Location = new System.Drawing.Point(464, 71);
            this.buttonProjectEdit.Name = "buttonProjectEdit";
            this.buttonProjectEdit.Size = new System.Drawing.Size(88, 39);
            this.buttonProjectEdit.TabIndex = 11;
            this.buttonProjectEdit.Text = "編集";
            this.buttonProjectEdit.UseVisualStyleBackColor = true;
            // 
            // buttonEditMembers
            // 
            this.buttonEditMembers.Location = new System.Drawing.Point(464, 135);
            this.buttonEditMembers.Name = "buttonEditMembers";
            this.buttonEditMembers.Size = new System.Drawing.Size(88, 39);
            this.buttonEditMembers.TabIndex = 11;
            this.buttonEditMembers.Text = "編集";
            this.buttonEditMembers.UseVisualStyleBackColor = true;
            this.buttonEditMembers.Click += new System.EventHandler(this.Button4_Click);
            // 
            // EditWorkItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 450);
            this.Controls.Add(this.buttonEditMembers);
            this.Controls.Add(this.buttonProjectEdit);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBoxAssignedMemer);
            this.Controls.Add(this.comboBoxProject);
            this.Controls.Add(this.textBoxTo);
            this.Controls.Add(this.textBoxFrom);
            this.Controls.Add(this.textBoxTags);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxWorkItemName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "EditWorkItem";
            this.Text = "EditWorkItem";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxWorkItemName;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBoxTags;
        private System.Windows.Forms.ComboBox comboBoxProject;
        private System.Windows.Forms.ComboBox comboBoxAssignedMemer;
        private System.Windows.Forms.TextBox textBoxFrom;
        private System.Windows.Forms.TextBox textBoxTo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonProjectEdit;
        private System.Windows.Forms.Button buttonEditMembers;
    }
}