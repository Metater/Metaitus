namespace MetaitusShared.Types
{
    public struct MVec2UL
    {
        public static MVec2UL zero = new MVec2UL(0, 0);

        public ulong x, y;

        public MVec2UL(ulong x, ulong y)
        {
            this.x = x;
            this.y = y;
        }

        public static MVec2UL operator +(MVec2UL a, MVec2UL b)
        {
            return new MVec2UL(a.x + b.x, a.y + b.y);
        }
        public static MVec2UL operator -(MVec2UL a, MVec2UL b)
        {
            return new MVec2UL(a.x - b.x, a.y - b.y);
        }
        public static MVec2UL operator *(MVec2UL a, MVec2UL b)
        {
            return new MVec2UL(a.x * b.x, a.y * b.y);
        }
        public static MVec2UL operator /(MVec2UL a, MVec2UL b)
        {
            return new MVec2UL(a.x / b.x, a.y / b.y);
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }
}