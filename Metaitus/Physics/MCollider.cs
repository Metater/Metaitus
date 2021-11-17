using Metaitus.Interfaces;
using Metaitus.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Metaitus.Physics
{
    public class MCollider
    {
        public bool IsStatic { get; protected set; }
        public MVec2F Min { get; protected set; }
        public MVec2F Max { get; protected set; }
        public MVec2F Offset { get; protected set; }
        public MVec2D Position { get; protected set; }
        public bool HasCollisionHandlers => collisionHandlers.Count != 0;

        private readonly List<ICollisionHandler> collisionHandlers = new List<ICollisionHandler>();

        public MCollider(MVec2F min, MVec2F max, MVec2F offset)
        {
            IsStatic = false;
            Min = min;
            Max = max;
            Offset = offset;
        }

        public MCollider(MVec2F min, MVec2F max, MVec2F offset, MVec2D position)
        {
            IsStatic = true;
            Min = min;
            Max = max;
            Offset = offset;
            Position = position;
        }

        public void AddCollisionHandler(ICollisionHandler collisionHandler)
        {
            collisionHandlers.Add(collisionHandler);
        }

        public void RemoveCollisionHandler(ICollisionHandler collisionHandler)
        {
            collisionHandlers.Remove(collisionHandler);
        }

        public bool Intersects(MVec2D position, MCollider other)
        {
            MVec2F otherPos = ((MVec2F)(other.Position - position)) - Offset + other.Offset;
            MVec2F otherMin = other.Min + otherPos;
            MVec2F otherMax = other.Max + otherPos;
            if (Max.x < otherMin.x || Min.x > otherMax.x) return false;
            if (Max.y < otherMin.y || Min.y > otherMax.y) return false;
            return true;
        }

        public bool Intersects(MLine other, out List<MVec2F> intersections)
        {
            intersections = new List<MVec2F>();

            MVec2F offsetMin = Min + Offset;
            MVec2F offsetMax = Max + Offset;

            float left = offsetMin.x;
            float right = left + offsetMax.x;
            float bottom = offsetMin.y;
            float top = bottom + offsetMax.y;

            MLine l = new MLine(new MVec2F(left, bottom), new MVec2F(left, top));
            MLine r = new MLine(new MVec2F(right, bottom), new MVec2F(right, top));
            MLine b = new MLine(new MVec2F(left, bottom), new MVec2F(right, bottom));
            MLine t = new MLine(new MVec2F(left, top), new MVec2F(right, top));

            bool intersects = false;
            if (l.Intersects(other, out MVec2F intersection))
            {
                intersects = true;
                intersections.Add(intersection);
            }
            if (r.Intersects(other, out intersection))
            {
                intersects = true;
                intersections.Add(intersection);
            }
            if (b.Intersects(other, out intersection))
            {
                intersects = true;
                intersections.Add(intersection);
            }
            if (t.Intersects(other, out intersection))
            {
                intersects = true;
                intersections.Add(intersection);
            }
            return intersects;
        }

        public bool ContainsPoint(MVec2D point)
        {
            return ContainsPoint(Position, point);
        }

        public bool ContainsPoint(MVec2D position, MVec2D point)
        {
            MVec2F pointPos = ((MVec2F)(point - position)) - Offset;
            float left = Min.x;
            float right = left + Max.x;
            float bottom = Min.y;
            float top = bottom + Max.y;
            return left <= pointPos.x && bottom <= pointPos.y && pointPos.x <= right && pointPos.y <= top;
        }

        public void Touched(MCollider toucher)
        {
            collisionHandlers.ForEach((h) => h.Touched(this, toucher));
        }
    }
    /*
    public abstract class MCollider
    {
        public MVec2F offset = MVec2F.zero;
        public bool isTrigger;
        public ICollisionHandler CollisionHandler { get; protected set; }
        public ulong Id { get; protected set; }
        public ushort[] Tags { get; protected set; }
    }

    public sealed class MAABBCollider : MCollider
    {
        public readonly MVec2F min;
        public readonly MVec2F max;

        public MAABBCollider(MVec2F min, MVec2F max, MVec2F offset, bool isTrigger, ICollisionHandler collisionHandler, ulong id, ushort[] tags = null)
        {
            this.min = min;
            this.max = max;
            base.offset = offset;
            base.isTrigger = isTrigger;
            CollisionHandler = collisionHandler;
            Id = id;
            Tags = tags;
        }
    }

    public sealed class MCircleCollider : MCollider
    {
        public readonly float radius;

        public MCircleCollider(float radius, MVec2F offset, ICollisionHandler collisionHandler, ulong id, ushort[] tags = null)
        {
            this.radius = radius;
            base.offset = offset;
            CollisionHandler = collisionHandler;
            Id = id;
            Tags = tags;
            isTrigger = true;
        }
    }
    */
}
