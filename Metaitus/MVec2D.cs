namespace Metaitus
{
    public struct MVec2D
    {
        public static MVec2D zero = new MVec2D(0, 0);

        public double x, y;

        public MVec2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
}