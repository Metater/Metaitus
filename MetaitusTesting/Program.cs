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
            WorldListener worldListener = new WorldListener();
            World world = new World(worldListener, 10);
            world.AddBody(new Body(Vector2.zero, new Vector2(1f, 10f)));
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
        }
    }
}
