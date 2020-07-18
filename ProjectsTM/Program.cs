using ProjectsTM.UI;
using System;
using System.Windows.Forms;

namespace ProjectsTM
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //if (VersionUpdateService.Update()) return;
            using (var mainForm = new MainForm())
            {
                Application.Run(mainForm);
            }
        }
    }
}
