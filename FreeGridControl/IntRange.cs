namespace FreeGridControl
{
    internal class IntRange
    {
        private int v1;
        private int v2;

        public IntRange(int low, int hight)
        {
            this.v1 = low;
            this.v2 = hight;
        }

        public bool Contains(int c)
        {
            return v1 <= c && c <= v2;
        }
    }
}