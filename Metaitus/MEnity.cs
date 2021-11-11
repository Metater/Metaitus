namespace Metaitus
{
    public class MEntity
    {
        public MVec2D pos;
        public MVec2F vel;

        public MEntity(MVec2D pos, MVec2F vel)
        {
            this.pos = pos;
            this.vel = vel;
        }
    }
}