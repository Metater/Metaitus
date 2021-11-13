using System;

namespace Metaitus.Types
{
    public struct MVec2F
    {
        public static MVec2F zero = new MVec2F(0, 0);

        public float x, y;

        public MVec2F(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static MVec2F operator +(MVec2F a, MVec2F b)
        {
            return new MVec2F(a.x + b.x, a.y + b.y);
        }
        public static MVec2F operator -(MVec2F a, MVec2F b)
        {
            return new MVec2F(a.x - b.x, a.y - b.y);
        }
        public static MVec2F operator *(MVec2F a, MVec2F b)
        {
            return new MVec2F(a.x * b.x, a.y * b.y);
        }
        public static MVec2F operator /(MVec2F a, MVec2F b)
        {
            return new MVec2F(a.x / b.x, a.y / b.y);
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }

        public static explicit operator MVec2F(MVec2D v)
        {
            return new MVec2F((float)v.x, (float)v.y);
        }
    }
}