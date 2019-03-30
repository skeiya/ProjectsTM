namespace TaskManagement
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.buttonPrintPreview = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonColorSetting = new System.Windows.Forms.Button();
            this.buttonImportCSV = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.buttonFilter = new System.Windows.Forms.Button();
            this.taskDrawAria = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.taskDrawAria)).BeginInit();
            this.SuspendLayout();
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // buttonPrintPreview
            // 
            this.buttonPrintPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPrintPreview.Location = new System.Drawing.Point(927, 619);
            this.buttonPrintPreview.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.buttonPrintPreview.Name = "buttonPrintPreview";
            this.buttonPrintPreview.Size = new System.Drawing.Size(225, 64);
            this.buttonPrintPreview.TabIndex = 2;
            this.buttonPrintPreview.Text = "印刷プレビュー";
            this.buttonPrintPreview.UseVisualStyleBackColor = true;
            this.buttonPrintPreview.Click += new System.EventHandler(this.buttonPrintPreview_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 628);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // buttonColorSetting
            // 
            this.buttonColorSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonColorSetting.Image = global::TaskManagement.Properties.Resources.icons8_swatch_48;
            this.buttonColorSetting.Location = new System.Drawing.Point(776, 619);
            this.buttonColorSetting.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.buttonColorSetting.Name = "buttonColorSetting";
            this.buttonColorSetting.Size = new System.Drawing.Size(64, 64);
            this.buttonColorSetting.TabIndex = 6;
            this.buttonColorSetting.UseVisualStyleBackColor = true;
            this.buttonColorSetting.Click += new System.EventHandler(this.buttonColorSetting_Click);
            // 
            // buttonImportCSV
            // 
            this.buttonImportCSV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonImportCSV.Location = new System.Drawing.Point(563, 619);
            this.buttonImportCSV.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.buttonImportCSV.Name = "buttonImportCSV";
            this.buttonImportCSV.Size = new System.Drawing.Size(134, 64);
            this.buttonImportCSV.TabIndex = 7;
            this.buttonImportCSV.Text = "CSVインポート...";
            this.buttonImportCSV.UseVisualStyleBackColor = true;
            this.buttonImportCSV.Click += new System.EventHandler(this.ButtonImportCSV_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExport.Location = new System.Drawing.Point(707, 619);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(64, 64);
            this.buttonExport.TabIndex = 9;
            this.buttonExport.Text = "RS";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // buttonFilter
            // 
            this.buttonFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFilter.Image = global::TaskManagement.Properties.Resources.icons8_filter_24;
            this.buttonFilter.Location = new System.Drawing.Point(845, 619);
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(64, 64);
            this.buttonFilter.TabIndex = 8;
            this.buttonFilter.UseVisualStyleBackColor = true;
            this.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
            // 
            // taskDrawAria
            // 
            this.taskDrawAria.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.taskDrawAria.Location = new System.Drawing.Point(13, 12);
            this.taskDrawAria.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.taskDrawAria.Name = "taskDrawAria";
            this.taskDrawAria.Size = new System.Drawing.Size(1139, 592);
            this.taskDrawAria.TabIndex = 1;
            this.taskDrawAria.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1163, 688);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.buttonFilter);
            this.Controls.Add(this.buttonImportCSV);
            this.Controls.Add(this.buttonColorSetting);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonPrintPreview);
            this.Controls.Add(this.taskDrawAria);
            this.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.taskDrawAria)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Drawing.Printing.PrintDocument printDocument;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.PictureBox taskDrawAria;
        private System.Windows.Forms.Button buttonPrintPreview;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonColorSetting;
        private System.Windows.Forms.Button buttonImportCSV;
        private System.Windows.Forms.Button buttonFilter;
        private System.Windows.Forms.Button buttonExport;
    }
}

