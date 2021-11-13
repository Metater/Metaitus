using Metaitus.Interfaces;
using Metaitus.Types;
using System.Collections.Generic;

namespace Metaitus.Physics
{
    public sealed class MEntity : ITickable
    {
        private readonly MZone zone;

        public MVec2D position;
        public MVec2D velocity;

        public readonly ulong id;
        public readonly MAABBCollider[] colliders;
        public readonly MAABBCollider[] aabbTriggers;
        public readonly MCircleCollider[] circleTriggers;

        public MCell Cell { get; private set; }

        private readonly List<MCell> pairs = new List<MCell>();

        public MEntity(MZone zone, MVec2D position, MVec2D velocity, ulong id, MAABBCollider[] colliders, MAABBCollider[] aabbTriggers, MCircleCollider[] circleTriggers)
        {
            this.zone = zone;
            this.position = position;
            this.velocity = velocity;
            this.id = id;
            this.colliders = colliders;
            this.aabbTriggers = aabbTriggers;
            this.circleTriggers = circleTriggers;
            Cell = zone.EnsureCell(position);
        }

        public void Tick(float timestep)
        {
            Move(timestep);
            Cell = zone.EnsureCell(position);
        }

        private bool Move(float timestep)
        {
            bool moved = true;
            MVec2D lastPos = position;
            position += velocity * new MVec2D(timestep, timestep);
            zone.GetCellAndSurrounding(position, pairs);
            foreach (MCell cell in pairs)
            {
                foreach (MStaticCollider staticCollider in cell.staticColliders)
                {
                    if (IsCollidingWithStatic(this, staticCollider, true))
                    {
                        position = lastPos;
                        position += velocity * new MVec2D(0, timestep);
                        if (IsCollidingWithStatic(this, staticCollider))
                        {
                            position = lastPos;
                            position += velocity * new MVec2D(timestep, 0);
                            if (IsCollidingWithStatic(this, staticCollider))
                            {
                                position = lastPos;
                                velocity = MVec2D.zero;
                                moved = false;
                            }
                            else velocity *= new MVec2D(1, 0);
                        }
                        else velocity *= new MVec2D(0, 1);
                    }
                }
            }
            pairs.Clear();
            return moved;
        }

        public static bool IsCollidingWithStatic(MEntity a, MStaticCollider b, bool sendTouched = false)
        {
            bool collided = false;
            foreach (MAABBCollider collider in a.colliders)
            {
                if (collided && collider.CollisionHandler == null) continue;
                MVec2F staticPos = ((MVec2F)(b.position - a.position)) - collider.offset;
                MVec2F staticMin = b.collider.min + staticPos;
                MVec2F staticMax = b.collider.max + staticPos;
                if (collider.max.x < staticMin.x || collider.min.x > staticMax.x) continue;
                if (collider.max.y < staticMin.y || collider.min.y > staticMax.y) continue;
                collided = true;
                if (sendTouched) collider.CollisionHandler?.Touched(collider, b.collider);
            }
            return collided;
        }
    }
}