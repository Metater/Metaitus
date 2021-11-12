using System;
using System.Collections.Generic;
using System.Text;

namespace Metaitus
{
    public class MQuadtree
    {
        public readonly MBox box;
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

        public MQuadtree AddCornerInsidePoint(uint x, uint y)
        {
            MQuadtree corner = null;
            for (int i = 0; i < 4; i++)
            {
                MBox cornerBox = box.GetCorner(i);
                if (cornerBox.PointInside(x, y))
                    corner = MQuadtree(cornerBox, this);
            }
            return corner;
        }
    }
}
