﻿using System;

namespace ProjectsTM.UI.Main
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows フォーム デザイナーで生成されたコード
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                    // _workItemGrid.Dispose();
                }

                // Dispose stuff here
                _fileIOService.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemReload = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSaveAsOtherName = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.詳細ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemExportRS = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemGenerateDummyData = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.編集ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemAddWorkItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemDivide = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemTaskList = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.表示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemColor = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemLargeRatio = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSmallRatio = new System.Windows.Forms.ToolStripMenuItem();
            this.管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemManageMember = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemMileStone = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemMySetting = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemTrendChart = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBoxFilter = new System.Windows.Forms.ToolStripComboBox();
            this.ToolStripMenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemHowToUse = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemVersion = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.menuStrip1.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
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
            this.詳細ToolStripMenuItem,
            this.toolStripMenuItemExit});
            this.ToolStripMenuItemFile.Name = "ToolStripMenuItemFile";
            this.ToolStripMenuItemFile.Size = new System.Drawing.Size(67, 23);
            this.ToolStripMenuItemFile.Text = "ファイル(&F)";
            // 
            // ToolStripMenuItemOpen
            // 
            this.ToolStripMenuItemOpen.Name = "ToolStripMenuItemOpen";
            this.ToolStripMenuItemOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.ToolStripMenuItemOpen.Size = new System.Drawing.Size(184, 22);
            this.ToolStripMenuItemOpen.Text = "開く(&O)";
            this.ToolStripMenuItemOpen.Click += new System.EventHandler(this.ToolStripMenuItemOpen_Click);
            // 
            // ToolStripMenuItemReload
            // 
            this.ToolStripMenuItemReload.Name = "ToolStripMenuItemReload";
            this.ToolStripMenuItemReload.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.ToolStripMenuItemReload.Size = new System.Drawing.Size(184, 22);
            this.ToolStripMenuItemReload.Text = "リロード(&R)";
            // 
            // ToolStripMenuItemSave
            // 
            this.ToolStripMenuItemSave.Name = "ToolStripMenuItemSave";
            this.ToolStripMenuItemSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.ToolStripMenuItemSave.Size = new System.Drawing.Size(184, 22);
            this.ToolStripMenuItemSave.Text = "上書き保存(&S)";
            // 
            // ToolStripMenuItemSaveAsOtherName
            // 
            this.ToolStripMenuItemSaveAsOtherName.Name = "ToolStripMenuItemSaveAsOtherName";
            this.ToolStripMenuItemSaveAsOtherName.Size = new System.Drawing.Size(184, 22);
            this.ToolStripMenuItemSaveAsOtherName.Text = "別名保存(&A)";
            // 
            // ToolStripMenuItemPrint
            // 
            this.ToolStripMenuItemPrint.Name = "ToolStripMenuItemPrint";
            this.ToolStripMenuItemPrint.Size = new System.Drawing.Size(184, 22);
            this.ToolStripMenuItemPrint.Text = "画像出力(&I)";
            // 
            // 詳細ToolStripMenuItem
            // 
            this.詳細ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemExportRS,
            this.ToolStripMenuItemGenerateDummyData});
            this.詳細ToolStripMenuItem.Name = "詳細ToolStripMenuItem";
            this.詳細ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.詳細ToolStripMenuItem.Text = "詳細(&D)";
            // 
            // ToolStripMenuItemExportRS
            // 
            this.ToolStripMenuItemExportRS.Name = "ToolStripMenuItemExportRS";
            this.ToolStripMenuItemExportRS.Size = new System.Drawing.Size(183, 22);
            this.ToolStripMenuItemExportRS.Text = "RSファイルをエクスポート";
            // 
            // ToolStripMenuItemGenerateDummyData
            // 
            this.ToolStripMenuItemGenerateDummyData.Name = "ToolStripMenuItemGenerateDummyData";
            this.ToolStripMenuItemGenerateDummyData.Size = new System.Drawing.Size(183, 22);
            this.ToolStripMenuItemGenerateDummyData.Text = "ダミーデータ生成";
            this.ToolStripMenuItemGenerateDummyData.Click += new System.EventHandler(this.ToolStripMenuItemGenerateDummyData_Click);
            // 
            // toolStripMenuItemExit
            // 
            this.toolStripMenuItemExit.Name = "toolStripMenuItemExit";
            this.toolStripMenuItemExit.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItemExit.Text = "終了(&X)";
            // 
            // 編集ToolStripMenuItem
            // 
            this.編集ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemAddWorkItem,
            this.ToolStripMenuItemDivide,
            this.ToolStripMenuItemTaskList,
            this.ToolStripMenuItemUndo,
            this.ToolStripMenuItemRedo});
            this.編集ToolStripMenuItem.Name = "編集ToolStripMenuItem";
            this.編集ToolStripMenuItem.Size = new System.Drawing.Size(57, 23);
            this.編集ToolStripMenuItem.Text = "編集(&E)";
            // 
            // ToolStripMenuItemAddWorkItem
            // 
            this.ToolStripMenuItemAddWorkItem.Name = "ToolStripMenuItemAddWorkItem";
            this.ToolStripMenuItemAddWorkItem.Size = new System.Drawing.Size(197, 22);
            this.ToolStripMenuItemAddWorkItem.Text = "作業項目の追加";
            // 
            // ToolStripMenuItemDivide
            // 
            this.ToolStripMenuItemDivide.Name = "ToolStripMenuItemDivide";
            this.ToolStripMenuItemDivide.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.ToolStripMenuItemDivide.Size = new System.Drawing.Size(197, 22);
            this.ToolStripMenuItemDivide.Text = "作業項目の分割";
            // 
            // ToolStripMenuItemTaskList
            // 
            this.ToolStripMenuItemTaskList.Name = "ToolStripMenuItemTaskList";
            this.ToolStripMenuItemTaskList.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.ToolStripMenuItemTaskList.Size = new System.Drawing.Size(197, 22);
            this.ToolStripMenuItemTaskList.Text = "タスクリスト";
            // 
            // ToolStripMenuItemUndo
            // 
            this.ToolStripMenuItemUndo.Name = "ToolStripMenuItemUndo";
            this.ToolStripMenuItemUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.ToolStripMenuItemUndo.Size = new System.Drawing.Size(197, 22);
            this.ToolStripMenuItemUndo.Text = "Undo";
            // 
            // ToolStripMenuItemRedo
            // 
            this.ToolStripMenuItemRedo.Name = "ToolStripMenuItemRedo";
            this.ToolStripMenuItemRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.ToolStripMenuItemRedo.Size = new System.Drawing.Size(197, 22);
            this.ToolStripMenuItemRedo.Text = "Redo";
            // 
            // 表示ToolStripMenuItem
            // 
            this.表示ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemFilter,
            this.ToolStripMenuItemColor,
            this.ToolStripMenuItemLargeRatio,
            this.ToolStripMenuItemSmallRatio});
            this.表示ToolStripMenuItem.Name = "表示ToolStripMenuItem";
            this.表示ToolStripMenuItem.Size = new System.Drawing.Size(58, 23);
            this.表示ToolStripMenuItem.Text = "表示(&V)";
            // 
            // ToolStripMenuItemFilter
            // 
            this.ToolStripMenuItemFilter.Image = global::ProjectsTM.UI.Main.Properties.Resources.icons8_filter_24;
            this.ToolStripMenuItemFilter.Name = "ToolStripMenuItemFilter";
            this.ToolStripMenuItemFilter.Size = new System.Drawing.Size(171, 22);
            this.ToolStripMenuItemFilter.Text = "フィルター(&F)";
            this.ToolStripMenuItemFilter.Click += new System.EventHandler(this.ToolStripMenuItemFilter_Click);
            // 
            // ToolStripMenuItemColor
            // 
            this.ToolStripMenuItemColor.Image = global::ProjectsTM.UI.Main.Properties.Resources.icons8_swatch_48;
            this.ToolStripMenuItemColor.Name = "ToolStripMenuItemColor";
            this.ToolStripMenuItemColor.Size = new System.Drawing.Size(171, 22);
            this.ToolStripMenuItemColor.Text = "色(&C)";
            this.ToolStripMenuItemColor.Click += new System.EventHandler(this.ToolStripMenuItemColor_Click);
            // 
            // ToolStripMenuItemLargeRatio
            // 
            this.ToolStripMenuItemLargeRatio.Name = "ToolStripMenuItemLargeRatio";
            this.ToolStripMenuItemLargeRatio.ShortcutKeyDisplayString = "Ctrl++";
            this.ToolStripMenuItemLargeRatio.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Oemplus)));
            this.ToolStripMenuItemLargeRatio.Size = new System.Drawing.Size(171, 22);
            this.ToolStripMenuItemLargeRatio.Text = "倍率(→大)";
            // 
            // ToolStripMenuItemSmallRatio
            // 
            this.ToolStripMenuItemSmallRatio.Name = "ToolStripMenuItemSmallRatio";
            this.ToolStripMenuItemSmallRatio.ShortcutKeyDisplayString = "Ctrl+ー";
            this.ToolStripMenuItemSmallRatio.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.OemMinus)));
            this.ToolStripMenuItemSmallRatio.Size = new System.Drawing.Size(171, 22);
            this.ToolStripMenuItemSmallRatio.Text = "倍率(→小)";
            // 
            // 管理ToolStripMenuItem
            // 
            this.管理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemManageMember,
            this.ToolStripMenuItemMileStone,
            this.ToolStripMenuItemMySetting,
            this.ToolStripMenuItemTrendChart});
            this.管理ToolStripMenuItem.Name = "管理ToolStripMenuItem";
            this.管理ToolStripMenuItem.Size = new System.Drawing.Size(62, 23);
            this.管理ToolStripMenuItem.Text = "管理(&M)";
            // 
            // ToolStripMenuItemManageMember
            // 
            this.ToolStripMenuItemManageMember.Name = "ToolStripMenuItemManageMember";
            this.ToolStripMenuItemManageMember.Size = new System.Drawing.Size(150, 22);
            this.ToolStripMenuItemManageMember.Text = "メンバー(&M)";
            this.ToolStripMenuItemManageMember.Click += new System.EventHandler(this.ToolStripMenuItemManageMember_Click);
            // 
            // ToolStripMenuItemMileStone
            // 
            this.ToolStripMenuItemMileStone.Name = "ToolStripMenuItemMileStone";
            this.ToolStripMenuItemMileStone.Size = new System.Drawing.Size(150, 22);
            this.ToolStripMenuItemMileStone.Text = "マイルストーン(&S)";
            this.ToolStripMenuItemMileStone.Click += new System.EventHandler(this.ToolStripMenuItemMileStone_Click);
            // 
            // ToolStripMenuItemMySetting
            // 
            this.ToolStripMenuItemMySetting.Name = "ToolStripMenuItemMySetting";
            this.ToolStripMenuItemMySetting.Size = new System.Drawing.Size(150, 22);
            this.ToolStripMenuItemMySetting.Text = "個人設定";
            this.ToolStripMenuItemMySetting.Click += new System.EventHandler(this.ToolStripMenuItemMySetting_Click);
            // 
            // ToolStripMenuItemTrendChart
            // 
            this.ToolStripMenuItemTrendChart.Name = "ToolStripMenuItemTrendChart";
            this.ToolStripMenuItemTrendChart.Size = new System.Drawing.Size(150, 22);
            this.ToolStripMenuItemTrendChart.Text = "総工数トレンド";
            // 
            // toolStripComboBoxFilter
            // 
            this.toolStripComboBoxFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxFilter.DropDownWidth = 150;
            this.toolStripComboBoxFilter.Name = "toolStripComboBoxFilter";
            this.toolStripComboBoxFilter.Size = new System.Drawing.Size(150, 23);
            // 
            // ToolStripMenuItemHelp
            // 
            this.ToolStripMenuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemHowToUse,
            this.ToolStripMenuItemVersion});
            this.ToolStripMenuItemHelp.Name = "ToolStripMenuItemHelp";
            this.ToolStripMenuItemHelp.Size = new System.Drawing.Size(65, 23);
            this.ToolStripMenuItemHelp.Text = "ヘルプ(&H)";
            // 
            // ToolStripMenuItemHowToUse
            // 
            this.ToolStripMenuItemHowToUse.Name = "ToolStripMenuItemHowToUse";
            this.ToolStripMenuItemHowToUse.Size = new System.Drawing.Size(167, 22);
            this.ToolStripMenuItemHowToUse.Text = "使い方(&H)...";
            // 
            // ToolStripMenuItemVersion
            // 
            this.ToolStripMenuItemVersion.Name = "ToolStripMenuItemVersion";
            this.ToolStripMenuItemVersion.Size = new System.Drawing.Size(167, 22);
            this.ToolStripMenuItemVersion.Text = "バージョン情報(&A)...";
            this.ToolStripMenuItemVersion.Click += new System.EventHandler(this.ToolStripMenuItemVersion_Click);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(537, 294);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 25);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(537, 319);
            this.toolStripContainer1.TabIndex = 11;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 344);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Name = "MainForm";
            this.Text = "ProjectsTM";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddWorkItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSave;
        private System.Windows.Forms.ToolStripMenuItem 表示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemFilter;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemColor;
        private System.Windows.Forms.ToolStripMenuItem 詳細ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemExportRS;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemLargeRatio;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSmallRatio;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemManageMember;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSaveAsOtherName;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemUndo;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemRedo;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxFilter;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemMileStone;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDivide;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemGenerateDummyData;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemReload;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemHowToUse;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemVersion;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemTaskList;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExit;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemMySetting;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemTrendChart;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
    }
}

