using System;
using System.Collections.Generic;
using System.Text;

namespace Metaitus
{
    public class MQuadtree
    {
        public MBox box;
        public readonly MQuadtree parent;
        public readonly MQuadtree[] tree = new MQuadtree[4];

        public MQuadtree(MBox root)
        {
            box = root;
            parent = null;
        }

        public MQuadtree(MBox box, MQuadtree parent)
        {
            this.box = box;
            this.parent = parent;
        }

        public MQuadtree AddCorner(int corner)
        {
            tree[corner] = box.GetCorner(corner);
            return tree[corner];
        }
    }
}
