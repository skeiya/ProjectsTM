using System.Windows.Forms;

namespace ProjectsTM.Logic
{
    static class KeyState
    {
        public static bool IsControlDown => (Control.ModifierKeys & Keys.Control) == Keys.Control;
    }
}
