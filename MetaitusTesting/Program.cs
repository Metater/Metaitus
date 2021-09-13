using System;
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
            world.AddBody(new Body(Vector2.zero, new Vector2(2.25f, 4.9f)));
            world.Start();
            while (!Console.KeyAvailable)
            {
                world.Update();
                Thread.Sleep(1);
            }
        }
    }
}
