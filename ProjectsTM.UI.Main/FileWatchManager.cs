using System;
using System.Windows.Forms;

namespace ProjectsTM.UI.Main
{
    class FileWatchManager
    {
        static bool _alreadyShow = false;
        private readonly Form _form;
        private readonly Action _reload;

        public FileWatchManager(Form form, Action reload)
        {
            this._form = form;
            this._reload = reload;
        }

        internal void ConfirmReload()
        {
            _form.BeginInvoke((Action)(() =>
            {
                if (_alreadyShow) return;
                _alreadyShow = true;
                var msg = "開いているファイルが外部で変更されました。リロードしますか？";
                if (MessageBox.Show(_form, msg, "message", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                _reload();
                _alreadyShow = false;
            }));
        }
    }
}
