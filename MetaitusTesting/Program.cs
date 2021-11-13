using System;
using System.Diagnostics;
using System.Threading;
using Metaitus;

namespace MetaitusTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            MZone zone = new MZone();
            zone.TryGetGrid(17592186044415d, 17592186044415d, out _);
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
