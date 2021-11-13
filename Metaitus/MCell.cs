using Metaitus.Interfaces;
using Metaitus.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Metaitus
{
    public class MCell
    {
        public readonly ulong index;
        public readonly List<MEntity> entities = new List<MEntity>();
        public readonly List<MStaticCollider> staticColliders = new List<MStaticCollider>();

        public MCell(ulong index)
        {
            this.index = index;
        }
    }
}
