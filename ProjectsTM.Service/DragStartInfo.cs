using FreeGridControl;
using System.Collections.Generic;

namespace ProjectsTM.Service
{
    public class DragStartInfo
    {
        public RawPoint Location { get; set; }
        public IEnumerable<ClientRectangle?> Rects { get; set; }
    }
}
