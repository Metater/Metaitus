using System;
using System.Collections.Generic;
using System.Text;

namespace Metaitus
{
    public abstract class Collider
    {
        public Body body;
        public Vector2 offset = Vector2.zero;
    }

    public class CircleCollider : Collider
    {
        public Vector2 pivot = new Vector2(0.5f, 0.5f);
        public float radius;

        public bool OverlapCircle(CircleCollider other)
        {
            float r = radius + other.radius;
            float x = (body.position.x + other.body.position.x);
            float y = (body.position.y + other.body.position.y);
            r *= r;
            x *= x;
            y *= y;
            return r < x + y;
        }
    }

    public class AABBCollider : Collider
    {
        public Vector2 pivot = new Vector2(0.5f, 0.5f);
        public Vector2 min;
        public Vector2 max;

        public bool OverlapAABB(AABBCollider other)
        {
            if (max.x < other.min.x || min.x > other.max.x) return false;
            if (max.y < other.min.y || min.y > other.max.y) return false;
            return true;
        }
    }

    public class AALineCollider : Collider
    {
        public bool alignedX;
        public float length;
        public float pivot = 0.5f;
    }
}
