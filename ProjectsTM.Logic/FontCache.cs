using System;
using System.Collections.Generic;
using System.Drawing;

namespace ProjectsTM.Logic
{
    public static class FontCache
    {
        private static Dictionary<Tuple<FontFamily, int, bool>, Font> _cache = new Dictionary<Tuple<FontFamily, int, bool>, Font>();
        public static Font GetFont(FontFamily family, int size, bool strikeout)
        {
            var key = new Tuple<FontFamily, int, bool>(family, size, strikeout);
            if (_cache.TryGetValue(key, out Font f)) return f;
            f = new Font(family, size, strikeout ? FontStyle.Strikeout : FontStyle.Regular);
            _cache.Add(key, f);
            return f;
        }
    }
}
