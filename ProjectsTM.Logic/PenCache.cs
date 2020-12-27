using System.Collections.Generic;
using System.Drawing;

namespace ProjectsTM.Logic
{
    public static class PenCache
    {
        private static Dictionary<Color, Pen> _cache = new Dictionary<Color, Pen>();
        public static Pen GetPen(Color c, float width)
        {
            if (_cache.TryGetValue(c, out var b)) return b;
            b = new Pen(c, width);
            _cache.Add(c, b);
            return b;
        }
    }
}
