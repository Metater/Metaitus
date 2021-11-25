using Metaitus.Interfaces;
using MetaitusShared.Types;
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

        public readonly List<MCollider> colliders;
        public readonly List<MTrigger> triggers;

        private readonly Func<MVec2D, float> GetDragFromVelocity;

        public readonly HashSet<string> tags = new HashSet<string>();

        public MCell Cell { get; private set; }

        private readonly List<MCell> cells = new List<MCell>(9);
        private readonly List<MCollider> biaxialIntersects;

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
        // optimizations ^^

        // cell caching if remove in good way???? may not work

        // later cache if other entities moved, use for sending collision events, if want to be really fast??

        // opptimization
        // with storing the static trigger intersects you have to account for the parent not existing or being deleted,
        // howeve works with statics no parent entity actually, just handle later
        // you dont need additional compliexity currently

        public MEntity(MZone zone, ulong id, MVec2D position, MVec2D velocity, List<MCollider> colliders, List<MTrigger> triggers, Func<MVec2D, float> GetDragFromVelocity = null)
        {
            this.zone = zone;
            this.id = id;

            Position = position;
            Velocity = velocity;

            this.colliders = colliders;
            colliders?.ForEach((c) => c.SetEntity(this));
            biaxialIntersects = new List<MCollider>(colliders == null ? 0 : colliders.Count);

            this.triggers = triggers;
            triggers?.ForEach((c) => c.SetEntity(this));

            this.GetDragFromVelocity = GetDragFromVelocity;

            Cell = zone.EnsureCell(position);
            Cell.entities.Add(this);
        }

        public void Tick(float timestep)
        {
            cells.Clear();
            zone.GetCellAndSurrounding(Position, cells);

            if (Velocity.x != 0 && Math.Abs(Velocity.x) < 0.0625d) Velocity *= new MVec2D(0, 1);
            if (Velocity.y != 0 && Math.Abs(Velocity.y) < 0.0625d) Velocity *= new MVec2D(1, 0);
            if (Velocity.x == 0 && Velocity.y == 0) // Entity cannot not move, has no velocity
            {

            }
            else if (Move(timestep))
            {
                MCell last = Cell;
                Cell = zone.EnsureCell(Position);
                // Optimization
                // Benefits of not fixing, certain surrounding cells cachable??
                // maybe not that important already fast lookup
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
                if (GetDragFromVelocity != null) Velocity *= 1 - (timestep * GetDragFromVelocity(Velocity));
            }
            else // Just stopped moving completely on both axes
            {

            }

            CheckTriggers();
        }

        // make another force that is added every tick and is scaled based on timestep?
        public void AddForce(MVec2D force)
        {
            Velocity += force;
        }

        private bool Move(float timestep)
        {
            if (colliders == null || colliders.Count == 0)
            {
                Position += Velocity * timestep;
                return true;
            }

            MVec2D usualPos = Position + (Velocity * timestep);
            MVec2D xDelta = new MVec2D(Velocity.x * timestep, 0);
            MVec2D yDelta = new MVec2D(0, Velocity.y * timestep);

            bool canMoveX = Velocity.x != 0;
            bool canMoveY = Velocity.y != 0;

            foreach (MCell cell in cells)
            {
                foreach (MCollider staticCollider in cell.staticColliders)
                {
                    if (IsColliding(usualPos, staticCollider, true))
                    {
                        if (canMoveX)
                        {
                            if (IsColliding(Position + xDelta, staticCollider, false))
                                canMoveX = false;
                        }
                        if (canMoveY)
                        {
                            if (IsColliding(Position + yDelta, staticCollider, false))
                                canMoveY = false;
                        }
                    }
                }
            }

            if (canMoveX) Position += xDelta;
            else Velocity *= new MVec2D(0, 1);
            if (canMoveY) Position += yDelta;
            else Velocity *= new MVec2D(1, 0);

            return canMoveX || canMoveY;
        }

        public void CheckTriggers()
        {
            foreach (MCell cell in cells)
            {
                foreach (MEntity entity in cell.entities)
                {
                    if (entity == this) continue;
                    foreach (MTrigger otherTrigger in entity.triggers)
                        CheckTriggersAgainstOther(otherTrigger);
                }
                foreach (MTrigger trigger in cell.staticTriggers)
                    CheckTriggersAgainstOther(trigger);
            }
        }

        public void CheckTriggersAgainstOther(MTrigger otherTrigger)
        {
            foreach (MTrigger trigger in triggers)
            {
                if (!trigger.HasTriggeredHandlers) continue;
                if (trigger.Intersects(Position, otherTrigger))
                    trigger.Triggered(otherTrigger);
            }
        }

        public bool IsColliding(MVec2D position, MCollider staticCollider, bool biaxial)
        {
            bool collided = false;
            if (biaxial)
            {
                biaxialIntersects.Clear();
                foreach (MCollider collider in colliders)
                {
                    if (collider.Intersects(position, staticCollider))
                    {
                        collided = true;
                        if (collider.HasCollisionHandlers) collider.Touched(staticCollider);
                        biaxialIntersects.Add(collider);
                    }
                }
            }
            else
            {
                foreach (MCollider collider in biaxialIntersects)
                {
                    if (collider.Intersects(position, staticCollider))
                    {
                        collided = true;
                        break;
                    }
                }
            }
            return collided;
        }
    }
}