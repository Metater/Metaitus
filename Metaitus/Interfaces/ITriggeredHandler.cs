using System;
using System.Collections.Generic;
using System.Text;

namespace Metaitus.Interfaces
{
    public interface ITriggeredHandler
    {
        void Triggered(MTrigger triggered, MTrigger triggerer);
    }
}
