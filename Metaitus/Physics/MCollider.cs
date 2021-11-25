using Metaitus.Interfaces;
using MetaitusShared.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Metaitus.Physics
{
    public class MCollider
    {

        // collider with list of positions it exists at
        // colliders that use use inheritance and dont contain unnessary things, but more complexity involved
        public bool IsStatic { get; protected set; }
        public MVec2F Min { get; protected set; }
        public MVec2F Max { get; protected set; }
        public MVec2F Offset { get; protected set; }
        public MVec2D Position { get; protected set; }
        public bool HasEntity => Entity != null;
        public MEntity Entity { get; protected set; }

        public readonly HashSet<string> tags = new HashSet<string>();

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

        public void SetEntity(MEntity entity)
        {
            Entity = entity;
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

            MLine l = new MLine(offsetMin, new MVec2F(offsetMin.x, offsetMax.y));
            MLine r = new MLine(new MVec2F(offsetMax.x, offsetMin.y), offsetMax);
            MLine b = new MLine(offsetMin, new MVec2F(offsetMax.x, offsetMin.y));
            MLine t = new MLine(new MVec2F(offsetMin.x, offsetMax.y), offsetMax);

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
}
