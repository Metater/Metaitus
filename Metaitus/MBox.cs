namespace Metaitus
{
    public struct MBox
    {
        // 1: BL, 2: TR
        public readonly uint x1, y1, x2, y2;
        public uint HalfLength => (x2 - x1) / 2;

        public MBox(uint x1, uint y1, uint x2, uint y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }

        public bool PointInside(uint x, uint y)
        {
            return (x >= x1 && x <= x2) && (y >= y1 && y <= y2);
        }

        public MBox GetCorner(int corner)
        {
            uint hl = HalfLength;
            switch (corner)
            {
                case 0:
                    return new MBox(x1, y1 + hl, x2 - hl, y2);
                case 1:
                    return new MBox(x1 + hl, y1 + hl, x2, y2);
                case 2:
                    return new MBox(x1, y1, x2 - hl, y2 - hl);
                default:
                    return new MBox(x1 + hl, y1, x2, y2 - hl);
            }
        }
    } 
}