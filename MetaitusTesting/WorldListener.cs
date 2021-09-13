using System;
using System.Collections.Generic;
using System.Text;
using Metaitus;

namespace MetaitusTesting
{
    public class WorldListener : IWorldListener
    {
        public void Message(string message)
        {
            Console.WriteLine(message);
        }

        public void NewBody(int bodyId)
        {
            Console.WriteLine($"New body: {bodyId}");
        }

        public void RemoveBody(int bodyId)
        {
            Console.WriteLine($"Remove body: {bodyId}");
        }

        public void UpdateBody(int bodyId, Vector2 newPosition)
        {
            Console.WriteLine($"Update body: {bodyId}, pos: {newPosition}");
        }
    }
}
