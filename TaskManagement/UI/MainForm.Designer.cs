using System;

namespace TaskManagement.UI
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemReload = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSaveAsOtherName = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.詳細ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemImportOldFile = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemExportRS = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemGenerateDummyData = new System.Windows.Forms.ToolStripMenuItem();
            this.編集ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemAddWorkItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemDivide = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.表示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemColor = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemLargerFont = new System.Windows.Forms.ToolStripMenuItem();
            this.フォント小ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemLargeRatio = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSmallRatio = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemDetail = new System.Windows.Forms.ToolStripMenuItem();
            this.管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemWorkingDas = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemManageMember = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemMileStone = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBoxFilter = new System.Windows.Forms.ToolStripComboBox();
            this.ToolStripMenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelSum = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelSelect = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelViewRatio = new System.Windows.Forms.ToolStripStatusLabel();
            this.workItemGrid1 = new TaskManagement.UI.WorkItemGrid();
            this.toolStripMenuItemTaskList = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemFile,
            this.編集ToolStripMenuItem,
            this.表示ToolStripMenuItem,
            this.管理ToolStripMenuItem,
            this.toolStripComboBoxFilter,
            this.ToolStripMenuItemHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(3, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(537, 25);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ToolStripMenuItemFile
            // 
            this.ToolStripMenuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemOpen,
            this.ToolStripMenuItemReload,
            this.ToolStripMenuItemSave,
            this.ToolStripMenuItemSaveAsOtherName,
            this.ToolStripMenuItemPrint,
            this.詳細ToolStripMenuItem});
            this.ToolStripMenuItemFile.Name = "ToolStripMenuItemFile";
            this.ToolStripMenuItemFile.Size = new System.Drawing.Size(53, 23);
            this.ToolStripMenuItemFile.Text = "ファイル";
            // 
            // ToolStripMenuItemOpen
            // 
            this.ToolStripMenuItemOpen.Name = "ToolStripMenuItemOpen";
            this.ToolStripMenuItemOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.ToolStripMenuItemOpen.Size = new System.Drawing.Size(170, 22);
            this.ToolStripMenuItemOpen.Text = "開く";
            this.ToolStripMenuItemOpen.Click += new System.EventHandler(this.ToolStripMenuItemOpen_Click);
            // 
            // ToolStripMenuItemReload
            // 
            this.ToolStripMenuItemReload.Name = "ToolStripMenuItemReload";
            this.ToolStripMenuItemReload.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.ToolStripMenuItemReload.Size = new System.Drawing.Size(170, 22);
            this.ToolStripMenuItemReload.Text = "リロード";
            this.ToolStripMenuItemReload.Click += new System.EventHandler(this.ToolStripMenuItemReload_Click);
            // 
            // ToolStripMenuItemSave
            // 
            this.ToolStripMenuItemSave.Name = "ToolStripMenuItemSave";
            this.ToolStripMenuItemSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.ToolStripMenuItemSave.Size = new System.Drawing.Size(170, 22);
            this.ToolStripMenuItemSave.Text = "上書き保存";
            this.ToolStripMenuItemSave.Click += new System.EventHandler(this.ToolStripMenuItemSave_Click);
            // 
            // ToolStripMenuItemSaveAsOtherName
            // 
            this.ToolStripMenuItemSaveAsOtherName.Name = "ToolStripMenuItemSaveAsOtherName";
            this.ToolStripMenuItemSaveAsOtherName.Size = new System.Drawing.Size(170, 22);
            this.ToolStripMenuItemSaveAsOtherName.Text = "別名保存";
            this.ToolStripMenuItemSaveAsOtherName.Click += new System.EventHandler(this.ToolStripMenuItemSaveAsOtherName_Click);
            // 
            // ToolStripMenuItemPrint
            // 
            this.ToolStripMenuItemPrint.Image = global::TaskManagement.Properties.Resources.icon_129150_48;
            this.ToolStripMenuItemPrint.Name = "ToolStripMenuItemPrint";
            this.ToolStripMenuItemPrint.Size = new System.Drawing.Size(170, 22);
            this.ToolStripMenuItemPrint.Text = "印刷";
            this.ToolStripMenuItemPrint.Click += new System.EventHandler(this.ToolStripMenuItemPrint_Click);
            // 
            // 詳細ToolStripMenuItem
            // 
            this.詳細ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemImportOldFile,
            this.ToolStripMenuItemExportRS,
            this.ToolStripMenuItemGenerateDummyData});
            this.詳細ToolStripMenuItem.Name = "詳細ToolStripMenuItem";
            this.詳細ToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.詳細ToolStripMenuItem.Text = "詳細";
            // 
            // ToolStripMenuItemImportOldFile
            // 
            this.ToolStripMenuItemImportOldFile.Name = "ToolStripMenuItemImportOldFile";
            this.ToolStripMenuItemImportOldFile.Size = new System.Drawing.Size(183, 22);
            this.ToolStripMenuItemImportOldFile.Text = "旧ファイルをインポート";
            this.ToolStripMenuItemImportOldFile.Click += new System.EventHandler(this.ToolStripMenuItemImportOldFile_Click);
            // 
            // ToolStripMenuItemExportRS
            // 
            this.ToolStripMenuItemExportRS.Name = "ToolStripMenuItemExportRS";
            this.ToolStripMenuItemExportRS.Size = new System.Drawing.Size(183, 22);
            this.ToolStripMenuItemExportRS.Text = "RSファイルをエクスポート";
            this.ToolStripMenuItemExportRS.Click += new System.EventHandler(this.ToolStripMenuItemExportRS_Click);
            // 
            // ToolStripMenuItemGenerateDummyData
            // 
            this.ToolStripMenuItemGenerateDummyData.Name = "ToolStripMenuItemGenerateDummyData";
            this.ToolStripMenuItemGenerateDummyData.Size = new System.Drawing.Size(183, 22);
            this.ToolStripMenuItemGenerateDummyData.Text = "ダミーデータ生成";
            this.ToolStripMenuItemGenerateDummyData.Click += new System.EventHandler(this.ToolStripMenuItemGenerateDummyData_Click);
            // 
            // 編集ToolStripMenuItem
            // 
            this.編集ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemAddWorkItem,
            this.ToolStripMenuItemDivide,
            this.ToolStripMenuItemSearch,
            this.ToolStripMenuItemUndo,
            this.ToolStripMenuItemRedo});
            this.編集ToolStripMenuItem.Name = "編集ToolStripMenuItem";
            this.編集ToolStripMenuItem.Size = new System.Drawing.Size(43, 23);
            this.編集ToolStripMenuItem.Text = "編集";
            // 
            // ToolStripMenuItemAddWorkItem
            // 
            this.ToolStripMenuItemAddWorkItem.Name = "ToolStripMenuItemAddWorkItem";
            this.ToolStripMenuItemAddWorkItem.Size = new System.Drawing.Size(156, 22);
            this.ToolStripMenuItemAddWorkItem.Text = "作業項目の追加";
            this.ToolStripMenuItemAddWorkItem.Click += new System.EventHandler(this.ToolStripMenuItemAddWorkItem_Click);
            // 
            // ToolStripMenuItemDivide
            // 
            this.ToolStripMenuItemDivide.Name = "ToolStripMenuItemDivide";
            this.ToolStripMenuItemDivide.Size = new System.Drawing.Size(156, 22);
            this.ToolStripMenuItemDivide.Text = "作業項目の分割";
            this.ToolStripMenuItemDivide.Click += new System.EventHandler(this.ToolStripMenuItemDivide_Click);
            // 
            // ToolStripMenuItemSearch
            // 
            this.ToolStripMenuItemSearch.Name = "ToolStripMenuItemSearch";
            this.ToolStripMenuItemSearch.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.ToolStripMenuItemSearch.Size = new System.Drawing.Size(156, 22);
            this.ToolStripMenuItemSearch.Text = "検索";
            this.ToolStripMenuItemSearch.Click += new System.EventHandler(this.ToolStripMenuItemSearch_Click);
            // 
            // ToolStripMenuItemUndo
            // 
            this.ToolStripMenuItemUndo.Name = "ToolStripMenuItemUndo";
            this.ToolStripMenuItemUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.ToolStripMenuItemUndo.Size = new System.Drawing.Size(156, 22);
            this.ToolStripMenuItemUndo.Text = "Undo";
            this.ToolStripMenuItemUndo.Click += new System.EventHandler(this.ToolStripMenuItemUndo_Click);
            // 
            // ToolStripMenuItemRedo
            // 
            this.ToolStripMenuItemRedo.Name = "ToolStripMenuItemRedo";
            this.ToolStripMenuItemRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.ToolStripMenuItemRedo.Size = new System.Drawing.Size(156, 22);
            this.ToolStripMenuItemRedo.Text = "Redo";
            this.ToolStripMenuItemRedo.Click += new System.EventHandler(this.ToolStripMenuItemRedo_Click);
            // 
            // 表示ToolStripMenuItem
            // 
            this.表示ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemFilter,
            this.ToolStripMenuItemColor,
            this.ToolStripMenuItemLargerFont,
            this.フォント小ToolStripMenuItem,
            this.ToolStripMenuItemLargeRatio,
            this.ToolStripMenuItemSmallRatio,
            this.ToolStripMenuItemDetail});
            this.表示ToolStripMenuItem.Name = "表示ToolStripMenuItem";
            this.表示ToolStripMenuItem.Size = new System.Drawing.Size(43, 23);
            this.表示ToolStripMenuItem.Text = "表示";
            // 
            // ToolStripMenuItemFilter
            // 
            this.ToolStripMenuItemFilter.Image = global::TaskManagement.Properties.Resources.icons8_filter_24;
            this.ToolStripMenuItemFilter.Name = "ToolStripMenuItemFilter";
            this.ToolStripMenuItemFilter.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemFilter.Text = "フィルター";
            this.ToolStripMenuItemFilter.Click += new System.EventHandler(this.ToolStripMenuItemFilter_Click);
            // 
            // ToolStripMenuItemColor
            // 
            this.ToolStripMenuItemColor.Image = global::TaskManagement.Properties.Resources.icons8_swatch_48;
            this.ToolStripMenuItemColor.Name = "ToolStripMenuItemColor";
            this.ToolStripMenuItemColor.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemColor.Text = "色";
            this.ToolStripMenuItemColor.Click += new System.EventHandler(this.ToolStripMenuItemColor_Click);
            // 
            // ToolStripMenuItemLargerFont
            // 
            this.ToolStripMenuItemLargerFont.Name = "ToolStripMenuItemLargerFont";
            this.ToolStripMenuItemLargerFont.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.ToolStripMenuItemLargerFont.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemLargerFont.Text = "フォント(→大)";
            this.ToolStripMenuItemLargerFont.Click += new System.EventHandler(this.ToolStripMenuItemLargerFont_Click);
            // 
            // フォント小ToolStripMenuItem
            // 
            this.フォント小ToolStripMenuItem.Name = "フォント小ToolStripMenuItem";
            this.フォント小ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.フォント小ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.フォント小ToolStripMenuItem.Text = "フォント(→小)";
            this.フォント小ToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItemSmallFont_Click);
            // 
            // ToolStripMenuItemLargeRatio
            // 
            this.ToolStripMenuItemLargeRatio.Name = "ToolStripMenuItemLargeRatio";
            this.ToolStripMenuItemLargeRatio.ShortcutKeyDisplayString = "Ctrl++";
            this.ToolStripMenuItemLargeRatio.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Oemplus)));
            this.ToolStripMenuItemLargeRatio.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemLargeRatio.Text = "倍率(→大)";
            this.ToolStripMenuItemLargeRatio.Click += new System.EventHandler(this.ToolStripMenuItemLargeRatio_Click);
            // 
            // ToolStripMenuItemSmallRatio
            // 
            this.ToolStripMenuItemSmallRatio.Name = "ToolStripMenuItemSmallRatio";
            this.ToolStripMenuItemSmallRatio.ShortcutKeyDisplayString = "Ctrl+ー";
            this.ToolStripMenuItemSmallRatio.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.OemMinus)));
            this.ToolStripMenuItemSmallRatio.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemSmallRatio.Text = "倍率(→小)";
            this.ToolStripMenuItemSmallRatio.Click += new System.EventHandler(this.ToolStripMenuItemSmallRatio_Click);
            // 
            // ToolStripMenuItemDetail
            // 
            this.ToolStripMenuItemDetail.Name = "ToolStripMenuItemDetail";
            this.ToolStripMenuItemDetail.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemDetail.Text = "詳細...";
            this.ToolStripMenuItemDetail.Click += new System.EventHandler(this.ToolStripMenuItemDetail_Click);
            // 
            // 管理ToolStripMenuItem
            // 
            this.管理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemWorkingDas,
            this.ToolStripMenuItemManageMember,
            this.ToolStripMenuItemMileStone,
            this.toolStripMenuItemTaskList});
            this.管理ToolStripMenuItem.Name = "管理ToolStripMenuItem";
            this.管理ToolStripMenuItem.Size = new System.Drawing.Size(43, 23);
            this.管理ToolStripMenuItem.Text = "管理";
            // 
            // ToolStripMenuItemWorkingDas
            // 
            this.ToolStripMenuItemWorkingDas.Name = "ToolStripMenuItemWorkingDas";
            this.ToolStripMenuItemWorkingDas.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemWorkingDas.Text = "稼働日";
            this.ToolStripMenuItemWorkingDas.Click += new System.EventHandler(this.ToolStripMenuItemWorkingDas_Click);
            // 
            // ToolStripMenuItemManageMember
            // 
            this.ToolStripMenuItemManageMember.Name = "ToolStripMenuItemManageMember";
            this.ToolStripMenuItemManageMember.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemManageMember.Text = "メンバー";
            this.ToolStripMenuItemManageMember.Click += new System.EventHandler(this.ToolStripMenuItemManageMember_Click);
            // 
            // ToolStripMenuItemMileStone
            // 
            this.ToolStripMenuItemMileStone.Name = "ToolStripMenuItemMileStone";
            this.ToolStripMenuItemMileStone.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemMileStone.Text = "マイルストーン";
            this.ToolStripMenuItemMileStone.Click += new System.EventHandler(this.ToolStripMenuItemMileStone_Click);
            // 
            // toolStripComboBoxFilter
            // 
            this.toolStripComboBoxFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxFilter.DropDownWidth = 150;
            this.toolStripComboBoxFilter.Name = "toolStripComboBoxFilter";
            this.toolStripComboBoxFilter.Size = new System.Drawing.Size(75, 23);
            // 
            // ToolStripMenuItemHelp
            // 
            this.ToolStripMenuItemHelp.Name = "ToolStripMenuItemHelp";
            this.ToolStripMenuItemHelp.Size = new System.Drawing.Size(48, 23);
            this.ToolStripMenuItemHelp.Text = "ヘルプ";
            this.ToolStripMenuItemHelp.Click += new System.EventHandler(this.ToolStripMenuItemHelp_Click_1);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelSum,
            this.toolStripStatusLabelSelect,
            this.toolStripStatusLabelViewRatio});
            this.statusStrip1.Location = new System.Drawing.Point(0, 322);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.statusStrip1.Size = new System.Drawing.Size(537, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelSum
            // 
            this.toolStripStatusLabelSum.Name = "toolStripStatusLabelSum";
            this.toolStripStatusLabelSum.Size = new System.Drawing.Size(103, 17);
            this.toolStripStatusLabelSum.Text = "SUM:0人日(0人月)";
            // 
            // toolStripStatusLabelSelect
            // 
            this.toolStripStatusLabelSelect.Name = "toolStripStatusLabelSelect";
            this.toolStripStatusLabelSelect.Size = new System.Drawing.Size(38, 17);
            this.toolStripStatusLabelSelect.Text = "Select";
            // 
            // toolStripStatusLabelViewRatio
            // 
            this.toolStripStatusLabelViewRatio.Name = "toolStripStatusLabelViewRatio";
            this.toolStripStatusLabelViewRatio.Size = new System.Drawing.Size(43, 17);
            this.toolStripStatusLabelViewRatio.Text = "拡大率";
            // 
            // toolStripMenuItemTaskList
            // 
            this.toolStripMenuItemTaskList.Name = "toolStripMenuItemTaskList";
            this.toolStripMenuItemTaskList.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItemTaskList.Text = "タスクリストを確認";
            this.toolStripMenuItemTaskList.Click += new System.EventHandler(this.toolStripMenuItemTaskList_Click);
            // 
            // workItemGrid1
            // 
            this.workItemGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workItemGrid1.FixedColCount = 0;
            this.workItemGrid1.FixedRowCount = 0;
            this.workItemGrid1.Location = new System.Drawing.Point(0, 25);
            this.workItemGrid1.LockUpdate = true;
            this.workItemGrid1.Name = "workItemGrid1";
            this.workItemGrid1.Size = new System.Drawing.Size(537, 297);
            this.workItemGrid1.TabIndex = 12;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 344);
            this.Controls.Add(this.workItemGrid1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Name = "MainForm";
            this.Text = "日程表ツール";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
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
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemLargeRatio;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSmallRatio;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemManageMember;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSaveAsOtherName;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemUndo;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemRedo;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxFilter;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSum;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSelect;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemMileStone;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDivide;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemGenerateDummyData;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDetail;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelViewRatio;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemReload;
        private WorkItemGrid workItemGrid1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTaskList;
    }
}

