using Metaitus.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Metaitus.Physics
{
    public class MStaticCollider
    {
        public readonly MVec2D position;
        public readonly MAABBCollider collider;

        public MStaticCollider(MVec2D position, MAABBCollider collider)
        {
            this.position = position;
            this.collider = collider;
        }
    }
}
