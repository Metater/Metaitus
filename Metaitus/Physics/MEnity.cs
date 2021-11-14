using Metaitus.Interfaces;
using Metaitus.Types;
using System;
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

        public float drag;

        public MCell Cell { get; private set; }

        private readonly List<MCell> pairs = new List<MCell>();

        // later support static entities, they cant move, colliders still maybe?

        // later do optimizations with doing EnsureCell checks, keep track of distance from center of cell and know
        // what timesteps with no extra speed increases make it impossible to leave
        // ^^^^ maybe not too expensive the way it is?


        // add a inverse bounding box to the actual entity, it is checked every tick
        // ^ have option

        // actually move entity between cells in the cell list

        // make player controller use forces, only way to not interfere with other forces

        // inverse camera movement, player is still everything else moves

        // better area for static cells there will be many just 1x1 cells you cant walk through

        // think about how client dll will work, new version of metatius needed?

        // make special "scenes" of collision that are loaded, then run a few checks then get deleted
        // ^^^^ for physics rollback

        // increase cell size if needed

        // figure out how to center objects, is the offsetting done correctly?

        public MEntity(MZone zone, MVec2D position, MVec2D velocity, ulong id, MAABBCollider[] colliders, MAABBCollider[] aabbTriggers, MCircleCollider[] circleTriggers, float drag = 0)
        {
            this.zone = zone;
            this.position = position;
            this.velocity = velocity;
            this.id = id;
            this.colliders = colliders;
            this.aabbTriggers = aabbTriggers;
            this.circleTriggers = circleTriggers;
            this.drag = drag;
            Cell = zone.EnsureCell(position);
        }

        public void Tick(float timestep)
        {
            if (!(Math.Abs(velocity.x) < 0.125d && Math.Abs(velocity.y) < 0.125d))
            {
                if (Move(timestep))
                {
                    MCell last = Cell;
                    Cell = zone.EnsureCell(position);
                    // Need a runtime static loading system later
                    //if (last != Cell && last.entities.Count == 0)
                        //zone.RemoveCell(last.index);
                }
                if (drag != 0) velocity *= 1 - (timestep * drag);
            }
            else
                velocity = MVec2D.zero;
        }

        public void AddForce(MVec2D force)
        {
            velocity += force;
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