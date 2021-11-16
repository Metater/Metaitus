public namespace Metaitus.Physics
{
    public struct MLine
    {
        public MVec2F a;
        public MVec2F b;

        public MLine(MVec2F a, MVec2F b)
        {
            this.a = a;
            this.b = b;
        }

        public static bool CCW(MVec2F a, MVec2F b, MVec2F c)
        {
            return (c.y - a.y) * (b.x - a.x) > (b.y - a.y) * (c.x - a.x);
        }

        public bool Intersects(MLine other)
        {
            return (CCW(a, other.a, other.b) != CCW(b, other.a, other.b)) && (CCW(a, b, other.a) != CCW(a, b, other.b));
        }
    }
}