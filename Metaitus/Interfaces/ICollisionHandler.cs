using Metaitus.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Metaitus.Interfaces
{
    public interface ICollisionHandler
    {
        void Touched(MAABBCollider touched, MAABBCollider toucher);
        void Triggered(MCollider triggered, MCollider triggerer);
    }
}
