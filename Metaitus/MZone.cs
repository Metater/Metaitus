using Metaitus.Physics;
using Metaitus.Types;
using System;
using System.Collections.Generic;

namespace Metaitus
{
    public class MZone
    {
        // this engine was created for kiss, keep it simple

        // have point in methods for aabbs and circles,
        // raymarch uses lerp, cycles closer and closer to find point

        // no ccd

        // aalines good idea, would be easy and already half implemented with the aabb logic

        // lines, edge colliders could work? just have line intersect methods for everything
        // ^ or maybe first they are just edge and line colliders and can only intersect with lines for simplicity

        // raymarching or marching to try to find closer stuff

        // premtive look for moving stuff, so no collision resolution needed, all are inelastic, just forces that push away

        // to look if a force is pushing away get cg vector, look at angle of lines, prob better way

        // quad tree, when expanding each caches other quad trees???

        // each entity keeps track of which quad tree its in

        // order: move, quad tree update, look for collisions

        // entities can keep track of when they could not possibly be out of their current tree and mark it,
        // bound to what forces the entity experiances
        // before another quad tree check is needed

        // sleep entities that are still and say they will always be in that quad tree until a sufficent force + dt wakes them up

        // moving entities, you can extrapolate their speed away from center,
        // figure out the largest dt then that is safe for them to stay in it, its a bounding box

        // figure out ways to make more quad trees, and do fewer checks on the quad trees

        // cg point ccd, center of gravity point continuous collision detection, used for fast moving bullets
        // that may jump through something relatively thin

        // quad tree cells are essentially aabbs, use for checks later

        // think about entity size and that effecting min quad tree cell size

        // cache all entities inside of each quad tree square

        // recursively despawn and spawn entities

        // think about how to implement sleeping entities, have a supplier for them?

        // point raycasts in select cells, for clicking on entities

        // add time scale

        // hold off on aabb triggers, do circle triggers

        public readonly double cellSize;
        // Not accounding for cellSize plus max
        public double MaxPosDimension => (2147483648d * cellSize) - 1d;
        public double MaxNegDimension => -2147483648d * cellSize;
        public ulong NextId => nextId++;

        private readonly Dictionary<ulong, MCell> grid = new Dictionary<ulong, MCell>();

        private ulong nextId = 0;

        public MZone(double cellSize)
        {
            this.cellSize = cellSize;
        }

        /*
        public MEntity SpawnEntity(MVec2D position, MVec2D velocity, MAABBCollider[] colliders, MAABBCollider[] aabbTriggers, MCircleCollider[] circleTriggers, float drag = 0)
        {
            MEntity entity = new MEntity(this, position, velocity, NextId, colliders, aabbTriggers, circleTriggers, drag);
            return entity;
        }
        */

        public MCell EnsureCell(MVec2D pos)
        {
            ulong index = GetCoordsIndex(pos);
            if (!grid.TryGetValue(index, out MCell cell))
            {
                cell = new MCell(index);
                grid.Add(index, cell);
            }
            return cell;
        }

        public bool RemoveCell(ulong index)
        {
            return grid.Remove(index);
        }

        public void GetCellAndSurrounding(MVec2D pos, List<MCell> cells)
        {
            // Doesn't account for overflows!
            MVec2UL coords = GetIntCoords(pos);
            ulong ulx = coords.x;
            ulong uly = coords.y;
            if (TryGetCell(new MVec2UL(ulx, uly), out MCell cell)) cells.Add(cell);
            if (TryGetCell(new MVec2UL(ulx - 1, uly + 1), out cell)) cells.Add(cell);
            if (TryGetCell(new MVec2UL(ulx, uly + 1), out cell)) cells.Add(cell);
            if (TryGetCell(new MVec2UL(ulx + 1, uly + 1), out cell)) cells.Add(cell);
            if (TryGetCell(new MVec2UL(ulx + 1, uly), out cell)) cells.Add(cell);
            if (TryGetCell(new MVec2UL(ulx + 1, uly - 1), out cell)) cells.Add(cell);
            if (TryGetCell(new MVec2UL(ulx, uly - 1), out cell)) cells.Add(cell);
            if (TryGetCell(new MVec2UL(ulx - 1, uly - 1), out cell)) cells.Add(cell);
            if (TryGetCell(new MVec2UL(ulx - 1, uly), out cell)) cells.Add(cell);
        }

        public bool TryGetCell(MVec2UL pos, out MCell cell)
        {
            return grid.TryGetValue(GetIndex(pos), out cell);
        }

        public static ulong GetCoordsIndex(MVec2D pos)
        {
            return GetIndex(GetIntCoords(pos));
        }

        public static MVec2UL GetIntCoords(MVec2D pos)
        {
            return new MVec2UL((ulong)((pos.x / cellSize) + 2147483648d), (ulong)((pos.y / cellSize) + 2147483648d));
        }

        public static ulong GetIndex(MVec2UL pos)
        {
            return (4294967296UL * pos.y) + pos.x;
        }

        public static MVec2D GetCenter(ulong index)
        {
            ulong ulx = index % 4294967296UL;
            ulong uly = index / 4294967296UL;

            double x = ((ulx - 2147483648d) * cellSize) + (cellSize / 2);
            double y = ((uly - 2147483648d) * cellSize) + (cellSize / 2);
            return new MVec2D(x, y);
        }
    }
}