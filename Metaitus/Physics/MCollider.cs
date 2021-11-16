using Metaitus.Interfaces;
using Metaitus.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Metaitus.Physics
{
    public class MCollider
    {
        public readonly bool isStatic;
        public MVec2F Min { get; protected set; }
        public MVec2F Max { get; protected set; }
        public MVec2F Offset { get; protected set; }
        public MVec2D Position { get; protected set; }
        public bool HasCollisionHandlers => collisionHandlers.Count != 0;

        private readonly List<ICollisionHandler> collisionHandlers = new List<ICollisionHandler>();

        public MCollider(MVec2F min, MVec2F max, MVec2F offset)
        {
            isStatic = false;
            Min = min;
            Max = max;
            Offset = offset;
        }

        public MCollider(MVec2F min, MVec2F max, MVec2D position, MVec2F offset)
        {
            isStatic = true;
            Min = min;
            Max = max;
            Position = position;
            Offset = offset;
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
            if (isStatic) return false;
            MVec2F otherPos = ((MVec2F)(other.Position - position)) - Offset + other.Offset;
            MVec2F otherMin = other.Min + otherPos;
            MVec2F otherMax = other.Max + otherPos;
            if (Max.x < otherMin.x || Min.x > otherMax.x) return false;
            if (Max.y < otherMin.y || Min.y > otherMax.y) return false;
            return true;
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
