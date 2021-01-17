using ProjectsTM.Service;
using System.Windows.Forms;

namespace ProjectsTM.UI.Main
{
    static class MainFormStateManager
    {
        internal static void Load(Form form)
        {
            FormWindowState state;
            state = FormSizeRestoreService.LoadLastTimeFormState("MainFormState");

            switch (state)
            {
                case FormWindowState.Maximized:
                    form.WindowState = state;
                    break;
                case FormWindowState.Normal:
                    form.Size = FormSizeRestoreService.LoadFormSize("MainFormSize");
                    break;
            }
        }

        internal static void Save(Form form)
        {
            FormSizeRestoreService.SaveFormSize(form.Height, form.Width, "MainFormSize");
            FormSizeRestoreService.SaveFormState(form.WindowState, "MainFormState");
        }
    }
}
