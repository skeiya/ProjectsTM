using System.Windows.Forms;

namespace ProjectsTM.Logic
{
    public static class KeyState
    {
        public static bool IsControlDown => (Control.ModifierKeys & Keys.Control) == Keys.Control;
        public static bool IsShiftDown => (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
    }
}
