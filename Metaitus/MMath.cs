using Metaitus.Types;

namespace Metaitus
{
    public static class MMath
    {
        public static float Lerp(float a, float b, float d)
        {
            return a * (1 - d) + b * d;
        }
        public static double Lerp(double a, double b, double d)
        {
            return a * (1 - d) + b * d;
        }
        public static MVec2F Lerp(MVec2F a, MVec2F b, float d)
        {
            float lerpX = Lerp(a.x, b.x, d);
            float lerpY = Lerp(a.y, b.y, d);
            return new MVec2F(lerpX, lerpY);
        }
        public static MVec2D Lerp(MVec2D a, MVec2D b, double d)
        {
            double lerpX = Lerp(a.x, b.x, d);
            double lerpY = Lerp(a.y, b.y, d);
            return new MVec2D(lerpX, lerpY);
        }
    }
}