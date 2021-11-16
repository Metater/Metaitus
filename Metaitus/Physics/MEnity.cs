using Metaitus.Interfaces;
using Metaitus.Types;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Metaitus.Physics
{
    public sealed class MEntity : ITickable
    {
        private readonly MZone zone;

        public readonly ulong id;

        public MVec2D Position { get; private set; }
        public MVec2D Velocity { get; private set; }

        public readonly MCollider[] colliders;

        public float drag;

        public MCell Cell { get; private set; }

        private readonly List<MCell> cells = new List<MCell>(9);
        private readonly List<MCollider> biaxialCollisions;

        // later support static entities, they cant move, colliders still maybe?

        // later do optimizations with doing EnsureCell checks, keep track of distance from center of cell and know
        // what timesteps with no extra speed increases make it impossible to leave
        // ^^^^ maybe not too expensive the way it is?


        // add a inverse bounding box to the actual entity, it is checked every tick
        // ^ have option

        // make player controller use forces, only way to not interfere with other forces

        // inverse camera movement, player is still everything else moves

        // better area for static cells there will be many just 1x1 cells you cant walk through

        // think about how client dll will work, new version of metatius needed?

        // make special "scenes" of collision that are loaded, then run a few checks then get deleted
        // ^^^^ for physics rollback

        // increase cell size if needed

        // figure out how to center objects, is the offsetting done correctly?

        // make different types of entites with inheritance,
        // give entity references to colliders?

        // add always applicable colliders, use inverse aabbs

        // bounding boxes, to see if a more complex object can collide with anything

        public MEntity(MZone zone, ulong id, MVec2D position, MVec2D velocity, MCollider[] colliders, float drag = 0)
        {
            this.zone = zone;
            this.id = id;
            Position = position;
            Velocity = velocity;
            this.colliders = colliders;
            biaxialCollisions = new List<MCollider>(colliders.Length);
            this.drag = drag;
            Cell = zone.EnsureCell(position);
            Cell.entities.Add(this);
        }

        public void Tick(float timestep)
        {
            if (Math.Abs(Velocity.x) < 0.125d && Math.Abs(Velocity.y) < 0.125d)
            {
                Velocity = MVec2D.zero;
            }
            else
            {
                if (Move(timestep))
                {
                    MCell last = Cell;
                    Cell = zone.EnsureCell(Position);
                    // Need a runtime static loading system later
                    if (last != Cell)
                    {
                        last.entities.Remove(this);
                        Cell.entities.Add(this);
                        if (last.entities.Count == 0)
                        {
                            //zone.RemoveCell(last.index);
                        }
                    }
                }
                if (drag != 0) Velocity *= 1 - (timestep * drag);
            }
        }

        public void AddForce(MVec2D force)
        {
            Velocity += force;
        }

        private bool Move(float timestep)
        {
            bool moved = true;
            MVec2D lastPos = Position;
            Position += Velocity * timestep;
            cells.Clear();
            zone.GetCellAndSurrounding(Position, cells);
            foreach (MCell cell in cells)
            {
                foreach (MCollider staticCollider in cell.staticColliders)
                {
                    if (IsColliding(staticCollider, true))
                    {
                        Position = lastPos;
                        Position += Velocity * new MVec2D(0, timestep);
                        if (IsColliding(staticCollider))
                        {
                            Position = lastPos;
                            Position += Velocity * new MVec2D(timestep, 0);
                            if (IsColliding(staticCollider))
                            {
                                Position = lastPos;
                                Velocity = MVec2D.zero;
                                moved = false;
                            }
                            else Velocity *= new MVec2D(1, 0);
                        }
                        else Velocity *= new MVec2D(0, 1);
                    }
                }
            }
            return moved;
        }

        public bool IsColliding(MCollider staticCollider, bool biaxial = false)
        {
            bool collided = false;
            if (biaxial)
            {
                biaxialCollisions.Clear();
                foreach (MCollider collider in colliders)
                {
                    if (collider.Intersects(Position, staticCollider))
                    {
                        collided = true;
                        if (collider.HasCollisionHandlers) collider.Touched(staticCollider);
                        biaxialCollisions.Add(collider);
                    }
                }
            }
            else
            {
                foreach (MCollider collider in biaxialCollisions)
                {
                    if (collided && !collider.HasCollisionHandlers) continue;
                    if (collider.Intersects(Position, staticCollider))
                    {
                        collided = true;
                        if (collider.HasCollisionHandlers) collider.Touched(staticCollider);
                    }
                }
            }
            return collided;
        }
    }
}