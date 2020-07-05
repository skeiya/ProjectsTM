﻿using System;
using System.Windows.Forms;
using TaskManagement.Logic;
using TaskManagement.ViewModel;

namespace TaskManagement.Service
{
    static class OldFileService
    {

        public static void ImportMemberAndWorkItems(ViewData viewData)
        {
            try
            {
                using (var dlg = new OpenFileDialog())
                {
                    if (dlg.ShowDialog() != DialogResult.OK) return;
                    viewData.Original.Members = CsvReader.ReadMembers(dlg.FileName);
                    viewData.Original.WorkItems = CsvReader.ReadWorkItems(dlg.FileName);
                    viewData.ClearCallenderAndMembers();
                    foreach (var w in viewData.Original.WorkItems) // TODO 暫定
                    {
                        viewData.UpdateCallenderAndMembers(w);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
