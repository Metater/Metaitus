using System;
using System.Diagnostics;
using System.Threading;
using Metaitus;
using Metaitus.Types;
using Metaitus.Physics;

namespace MetaitusTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(MZone.GetIntCoords(new MVec2D(17179869183d, 17179869183d)));
            //ulong i = MZone.GetCoordsIndex(new MVec2D(0, 0));
            //Console.WriteLine(i);
            //Console.WriteLine(MZone.GetCenter(i));

            MLine a = new MLine(new MVec2F(0, 0), new MVec2F(4, 4));
            MLine b = new MLine(new MVec2F(0, 4), new MVec2F(4, 0));
            MLine c = new MLine(new MVec2F(1, 0), new MVec2F(4, 0));
            Console.WriteLine(a.Intersects(b, out MVec2F pt));
            Console.WriteLine(pt);
            Console.WriteLine(a.Intersects(c, out pt));
            Console.WriteLine(pt);
            /*
            WorldListener worldListener = new WorldListener();
            World world = new World(worldListener, 10);

            Body body = new Body()
                .SetPosition(Vector2.zero)
                .SetVelocity(new Vector2(1f, 10f))
                .SetDrag(true, 1)
                .SetGravity(true, 1);

            world.AddBody(body);
            Stopwatch deltaTimeStopwatch = new Stopwatch();
            deltaTimeStopwatch.Start();
            while (!Console.KeyAvailable)
            {
                deltaTimeStopwatch.Stop();
                float deltaTime = (float)deltaTimeStopwatch.Elapsed.TotalSeconds;
                deltaTimeStopwatch.Restart();
                world.Update(deltaTime);
                Thread.Sleep(1);
            }
            */

            // For circles colliding, radius^2+radius^2 >= distance^2 between circles
        }
    }
}
