using System.Collections.Generic;
using System.Drawing;

namespace ProjectsTM.UI
{
    class BrushCache
    {
        private static Dictionary<Color, Brush> _cache = new Dictionary<Color, Brush>();
        public static Brush GetBrush(Color c)
        {
            if (_cache.TryGetValue(c, out Brush b)) return b;
            b = new SolidBrush(c);
            _cache.Add(c, b);
            return b;
        }
    }
}
