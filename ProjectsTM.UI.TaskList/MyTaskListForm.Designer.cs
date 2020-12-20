
namespace ProjectsTM.UI.TaskList
{
    partial class MyTaskListForm
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
            this._myTasklistView = new System.Windows.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderProject = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPeriod = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // _myTasklistView
            // 
            this._myTasklistView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._myTasklistView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderProject,
            this.columnHeaderPeriod,
            this.columnHeaderState});
            this._myTasklistView.HideSelection = false;
            this._myTasklistView.Location = new System.Drawing.Point(17, 12);
            this._myTasklistView.Name = "_myTasklistView";
            this._myTasklistView.Size = new System.Drawing.Size(493, 392);
            this._myTasklistView.TabIndex = 2;
            this._myTasklistView.UseCompatibleStateImageBehavior = false;
            this._myTasklistView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "名前";
            this.columnHeaderName.Width = 150;
            // 
            // columnHeaderProject
            // 
            this.columnHeaderProject.Text = "プロジェクト";
            this.columnHeaderProject.Width = 120;
            // 
            // columnHeaderPeriod
            // 
            this.columnHeaderPeriod.Text = "期日";
            this.columnHeaderPeriod.Width = 120;
            // 
            // columnHeaderState
            // 
            this.columnHeaderState.Text = "状態";
            this.columnHeaderState.Width = 100;
            // 
            // MyTaskListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(522, 416);
            this.Controls.Add(this._myTasklistView);
            this.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MyTaskListForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView _myTasklistView;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderProject;
        private System.Windows.Forms.ColumnHeader columnHeaderPeriod;
        private System.Windows.Forms.ColumnHeader columnHeaderState;
    }
}