namespace Metaitus
{
    public struct MBox
    {
        // 1: BL, 2: TR
        public uint x1, y1, x2, y2;

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
            // 0 1
            // 2 3
            switch (corner)
            {
                case 0:
                    return new MBox()
                    break;
                case 2:

                    break;
                case 3:

                    break;
                default:
                    
                    break;
            }
        }
    } 
}