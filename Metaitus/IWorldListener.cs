using System;
using System.Collections.Generic;
using System.Text;

namespace Metaitus
{
    public interface IWorldListener
    {
        void Message(string message);
        void NewBody(int bodyId);
        void UpdateBody(int bodyId, Vector2 newPosition);
        void RemoveBody(int bodyId);
    }
}
