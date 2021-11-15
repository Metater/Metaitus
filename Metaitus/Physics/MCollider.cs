using Metaitus.Interfaces;
using Metaitus.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Metaitus.Physics
{
    public class MCollider
    {

    }
    public class MDynamicCollider
    {

    }
    public class MStaticCollider
    {
        
    }
    public abstract class MCollider
    {
        public MVec2F offset = MVec2F.zero;
        public bool isTrigger;
        public ICollisionHandler CollisionHandler { get; protected set; }
        public ulong Id { get; protected set; }
        public ushort[] Tags { get; protected set; }

        public MVec2F ApplyOffset(MVec2F position)
        {
            return position + offset;
        }
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
}
