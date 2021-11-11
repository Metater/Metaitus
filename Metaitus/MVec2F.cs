namespace Metaitus
{
    public struct MVec2F
    {
        public const MVec2F zero = new MVec2F(0, 0);

        public float x, y;

        public MVec2F(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
}