using System.Numerics;

namespace Metaitus.Types
{
    public struct MVec2D
    {
        public static MVec2D zero = new MVec2D(0, 0);

        public double x, y;

        public double SqrMagnitude => (x * x) + (y * y);

        public MVec2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static MVec2D operator +(MVec2D a, MVec2D b)
        {
            return new MVec2D(a.x + b.x, a.y + b.y);
        }
        public static MVec2D operator -(MVec2D a, MVec2D b)
        {
            return new MVec2D(a.x - b.x, a.y - b.y);
        }
        public static MVec2D operator *(MVec2D a, MVec2D b)
        {
            return new MVec2D(a.x * b.x, a.y * b.y);
        }
        public static MVec2D operator /(MVec2D a, MVec2D b)
        {
            return new MVec2D(a.x / b.x, a.y / b.y);
        }

        public static MVec2D operator *(MVec2D a, double b)
        {
            return new MVec2D(a.x * b, a.y * b);
        }
        public static MVec2D operator *(double a, MVec2D b)
        {
            return new MVec2D(b.x * a, b.y * a);
        }

        public double Dot(MVec2D v)
        {
            return (x * v.x) + (y * v.y);
        }

        public double Cross(MVec2D v)
        {
            return (x * v.y) - (y * v.x);
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }

        public static explicit operator MVec2D(MVec2F v)
        {
            return new MVec2D(v.x, v.y);
        }
    }
}