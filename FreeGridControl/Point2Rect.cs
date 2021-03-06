﻿using System;
using System.Drawing;

namespace FreeGridControl
{
    public static class Point2Rect
    {
        public static ClientRectangle GetRectangle(Point p1, Point p2)
        {
            var x = Math.Min(p1.X, p2.X);
            var w = Math.Abs(p1.X - p2.X);
            var y = Math.Min(p1.Y, p2.Y);
            var h = Math.Abs(p1.Y - p2.Y);
            return new ClientRectangle(x, y, w, h);
        }
    }
}
