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

        MQuadtree tree = new MQuadtree(new MBox(0, 0, 4294967295, 4294967295));
    }
}