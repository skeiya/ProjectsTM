using System;

namespace FreeGridControl
{
    public struct RawPoint : IEquatable<RawPoint>
    {
        public int X { get; }
        public int Y { get; }
        public RawPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object other)
        {
            return other is RawPoint otherRawPoint && Equals(otherRawPoint);
        }

        public override int GetHashCode()
        {
            int hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        public bool Equals(RawPoint other)
        {
            return X == other.X && Y == other.Y;
        }

        public static bool operator ==(RawPoint p1, RawPoint p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(RawPoint p1, RawPoint p2)
        {
            return !p1.Equals(p2);
        }
    }
}
