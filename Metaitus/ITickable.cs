using System;
using System.Collections.Generic;
using System.Text;

namespace Metaitus
{
    public interface ITickable
    {
        void Tick(float timestep);
    }
}
