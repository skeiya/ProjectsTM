using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectsTM.Logic
{
    static class KeyState
    {
        public static bool IsControlDown => (Control.ModifierKeys & Keys.Control) == Keys.Control;
    }
}
