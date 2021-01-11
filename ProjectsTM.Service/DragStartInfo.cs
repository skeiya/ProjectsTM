using FreeGridControl;
using System.Collections.Generic;

namespace ProjectsTM.Service
{
    public class DragStartInfo
    {
        public DragStartInfo(RawPoint location, IEnumerable<ClientRectangle?> rects)
        {
            Location = location;
            Rects = rects;
        }

        public RawPoint Location { get; set; }
        public IEnumerable<ClientRectangle?> Rects { get; set; }
    }
}
