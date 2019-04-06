using System;

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
            this.taskDrawAria = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.詳細ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemImportOldFile = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemExportRS = new System.Windows.Forms.ToolStripMenuItem();
            this.編集ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemAddWorkItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.表示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemColor = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemLargerFont = new System.Windows.Forms.ToolStripMenuItem();
            this.フォント小ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.倍率ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemWorkingDas = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.taskDrawAria)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
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
            // taskDrawAria
            // 
            this.taskDrawAria.BackColor = System.Drawing.Color.White;
            this.taskDrawAria.Location = new System.Drawing.Point(24, 17);
            this.taskDrawAria.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.taskDrawAria.Name = "taskDrawAria";
            this.taskDrawAria.Size = new System.Drawing.Size(1019, 543);
            this.taskDrawAria.TabIndex = 1;
            this.taskDrawAria.TabStop = false;
            this.taskDrawAria.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TaskDrawAria_MouseDoubleClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemFile,
            this.編集ToolStripMenuItem,
            this.表示ToolStripMenuItem,
            this.管理ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1163, 42);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ToolStripMenuItemFile
            // 
            this.ToolStripMenuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemOpen,
            this.ToolStripMenuItemSave,
            this.ToolStripMenuItemPrint,
            this.詳細ToolStripMenuItem});
            this.ToolStripMenuItemFile.Name = "ToolStripMenuItemFile";
            this.ToolStripMenuItemFile.Size = new System.Drawing.Size(95, 38);
            this.ToolStripMenuItemFile.Text = "ファイル";
            // 
            // ToolStripMenuItemOpen
            // 
            this.ToolStripMenuItemOpen.Name = "ToolStripMenuItemOpen";
            this.ToolStripMenuItemOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.ToolStripMenuItemOpen.Size = new System.Drawing.Size(243, 38);
            this.ToolStripMenuItemOpen.Text = "開く";
            this.ToolStripMenuItemOpen.Click += new System.EventHandler(this.ToolStripMenuItemOpen_Click);
            // 
            // ToolStripMenuItemSave
            // 
            this.ToolStripMenuItemSave.Name = "ToolStripMenuItemSave";
            this.ToolStripMenuItemSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.ToolStripMenuItemSave.Size = new System.Drawing.Size(243, 38);
            this.ToolStripMenuItemSave.Text = "保存";
            this.ToolStripMenuItemSave.Click += new System.EventHandler(this.ToolStripMenuItemSave_Click);
            // 
            // ToolStripMenuItemPrint
            // 
            this.ToolStripMenuItemPrint.Image = global::TaskManagement.Properties.Resources.icon_129150_48;
            this.ToolStripMenuItemPrint.Name = "ToolStripMenuItemPrint";
            this.ToolStripMenuItemPrint.Size = new System.Drawing.Size(243, 38);
            this.ToolStripMenuItemPrint.Text = "印刷";
            this.ToolStripMenuItemPrint.Click += new System.EventHandler(this.ToolStripMenuItemPrint_Click);
            // 
            // 詳細ToolStripMenuItem
            // 
            this.詳細ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemImportOldFile,
            this.ToolStripMenuItemExportRS});
            this.詳細ToolStripMenuItem.Name = "詳細ToolStripMenuItem";
            this.詳細ToolStripMenuItem.Size = new System.Drawing.Size(243, 38);
            this.詳細ToolStripMenuItem.Text = "詳細";
            // 
            // ToolStripMenuItemImportOldFile
            // 
            this.ToolStripMenuItemImportOldFile.Name = "ToolStripMenuItemImportOldFile";
            this.ToolStripMenuItemImportOldFile.Size = new System.Drawing.Size(333, 38);
            this.ToolStripMenuItemImportOldFile.Text = "旧ファイルをインポート";
            this.ToolStripMenuItemImportOldFile.Click += new System.EventHandler(this.ToolStripMenuItemImportOldFile_Click);
            // 
            // ToolStripMenuItemExportRS
            // 
            this.ToolStripMenuItemExportRS.Name = "ToolStripMenuItemExportRS";
            this.ToolStripMenuItemExportRS.Size = new System.Drawing.Size(333, 38);
            this.ToolStripMenuItemExportRS.Text = "RSファイルをエクスポート";
            this.ToolStripMenuItemExportRS.Click += new System.EventHandler(this.ToolStripMenuItemExportRS_Click);
            // 
            // 編集ToolStripMenuItem
            // 
            this.編集ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemAddWorkItem,
            this.ToolStripMenuItemSearch});
            this.編集ToolStripMenuItem.Name = "編集ToolStripMenuItem";
            this.編集ToolStripMenuItem.Size = new System.Drawing.Size(75, 38);
            this.編集ToolStripMenuItem.Text = "編集";
            // 
            // ToolStripMenuItemAddWorkItem
            // 
            this.ToolStripMenuItemAddWorkItem.Name = "ToolStripMenuItemAddWorkItem";
            this.ToolStripMenuItemAddWorkItem.Size = new System.Drawing.Size(278, 38);
            this.ToolStripMenuItemAddWorkItem.Text = "作業項目の追加";
            this.ToolStripMenuItemAddWorkItem.Click += new System.EventHandler(this.ToolStripMenuItemAddWorkItem_Click);
            // 
            // ToolStripMenuItemSearch
            // 
            this.ToolStripMenuItemSearch.Name = "ToolStripMenuItemSearch";
            this.ToolStripMenuItemSearch.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.ToolStripMenuItemSearch.Size = new System.Drawing.Size(278, 38);
            this.ToolStripMenuItemSearch.Text = "検索";
            this.ToolStripMenuItemSearch.Click += new System.EventHandler(this.ToolStripMenuItemSearch_Click);
            // 
            // 表示ToolStripMenuItem
            // 
            this.表示ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemFilter,
            this.ToolStripMenuItemColor,
            this.ToolStripMenuItemLargerFont,
            this.フォント小ToolStripMenuItem,
            this.倍率ToolStripMenuItem});
            this.表示ToolStripMenuItem.Name = "表示ToolStripMenuItem";
            this.表示ToolStripMenuItem.Size = new System.Drawing.Size(75, 38);
            this.表示ToolStripMenuItem.Text = "表示";
            // 
            // ToolStripMenuItemFilter
            // 
            this.ToolStripMenuItemFilter.Image = global::TaskManagement.Properties.Resources.icons8_filter_24;
            this.ToolStripMenuItemFilter.Name = "ToolStripMenuItemFilter";
            this.ToolStripMenuItemFilter.Size = new System.Drawing.Size(328, 38);
            this.ToolStripMenuItemFilter.Text = "フィルター";
            this.ToolStripMenuItemFilter.Click += new System.EventHandler(this.ToolStripMenuItemFilter_Click);
            // 
            // ToolStripMenuItemColor
            // 
            this.ToolStripMenuItemColor.Image = global::TaskManagement.Properties.Resources.icons8_swatch_48;
            this.ToolStripMenuItemColor.Name = "ToolStripMenuItemColor";
            this.ToolStripMenuItemColor.Size = new System.Drawing.Size(328, 38);
            this.ToolStripMenuItemColor.Text = "色";
            this.ToolStripMenuItemColor.Click += new System.EventHandler(this.ToolStripMenuItemColor_Click);
            // 
            // ToolStripMenuItemLargerFont
            // 
            this.ToolStripMenuItemLargerFont.Name = "ToolStripMenuItemLargerFont";
            this.ToolStripMenuItemLargerFont.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.ToolStripMenuItemLargerFont.Size = new System.Drawing.Size(328, 38);
            this.ToolStripMenuItemLargerFont.Text = "フォント(→大)";
            this.ToolStripMenuItemLargerFont.Click += new System.EventHandler(this.ToolStripMenuItemLargerFont_Click);
            // 
            // フォント小ToolStripMenuItem
            // 
            this.フォント小ToolStripMenuItem.Name = "フォント小ToolStripMenuItem";
            this.フォント小ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.フォント小ToolStripMenuItem.Size = new System.Drawing.Size(328, 38);
            this.フォント小ToolStripMenuItem.Text = "フォント(→小)";
            this.フォント小ToolStripMenuItem.Click += new System.EventHandler(this.フォント小ToolStripMenuItem_Click);
            // 
            // 倍率ToolStripMenuItem
            // 
            this.倍率ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3});
            this.倍率ToolStripMenuItem.Name = "倍率ToolStripMenuItem";
            this.倍率ToolStripMenuItem.Size = new System.Drawing.Size(328, 38);
            this.倍率ToolStripMenuItem.Text = "倍率";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(173, 38);
            this.toolStripMenuItem2.Text = "100%";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.ToolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(173, 38);
            this.toolStripMenuItem3.Text = "200%";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.ToolStripMenuItem3_Click);
            // 
            // 管理ToolStripMenuItem
            // 
            this.管理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemWorkingDas});
            this.管理ToolStripMenuItem.Name = "管理ToolStripMenuItem";
            this.管理ToolStripMenuItem.Size = new System.Drawing.Size(75, 38);
            this.管理ToolStripMenuItem.Text = "管理";
            // 
            // ToolStripMenuItemWorkingDas
            // 
            this.ToolStripMenuItemWorkingDas.Name = "ToolStripMenuItemWorkingDas";
            this.ToolStripMenuItemWorkingDas.Size = new System.Drawing.Size(324, 38);
            this.ToolStripMenuItemWorkingDas.Text = "稼働日";
            this.ToolStripMenuItemWorkingDas.Click += new System.EventHandler(this.ToolStripMenuItemWorkingDas_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Location = new System.Drawing.Point(0, 666);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1163, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.taskDrawAria);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 42);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1163, 624);
            this.panel1.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1163, 688);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.Name = "Form1";
            this.Text = "TaskManager";
            ((System.ComponentModel.ISupportInitialize)(this.taskDrawAria)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Drawing.Printing.PrintDocument printDocument;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.PictureBox taskDrawAria;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemFile;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemOpen;
        private System.Windows.Forms.ToolStripMenuItem 編集ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemPrint;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddWorkItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSave;
        private System.Windows.Forms.ToolStripMenuItem 表示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemFilter;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemColor;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemLargerFont;
        private System.Windows.Forms.ToolStripMenuItem フォント小ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSearch;
        private System.Windows.Forms.ToolStripMenuItem 詳細ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemImportOldFile;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemExportRS;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemWorkingDas;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem 倍率ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
    }
}

