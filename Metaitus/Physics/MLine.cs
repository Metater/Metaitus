using MetaitusShared.Types;

namespace Metaitus.Physics
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

        // https://www.codeproject.com/Tips/862988/Find-the-Intersection-Point-of-Two-Line-Segments
        public bool Intersects(MLine other, out MVec2F intersection)
        {
            intersection = new MVec2F(float.NaN, float.NaN);

            var r = b - a;
            var s = other.b - other.a;
            var rxs = r.Cross(s);
            if (rxs.IsZero()) return false;

            var v = (other.a - a);
            var qpxr = v.Cross(r);
            var t = v.Cross(s) / rxs;
            var u = qpxr / rxs;

            if ((0 <= t && t <= 1) && (0 <= u && u <= 1))
            {
                intersection = a + (t * r);
                return true;
            }

            return false;
        }
    }
}