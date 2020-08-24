using FreeGridControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsTM.UI.MainForm
{
    class RawRange
    {
        public RowIndex Row { get; private set; }
        public int Count { get; private set; }

        public RawRange(RowIndex row, int count)
        {
            this.Row = row;
            this.Count = count;
        }
    }
}
