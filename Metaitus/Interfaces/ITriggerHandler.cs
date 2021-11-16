using System;
using System.Collections.Generic;
using System.Text;

namespace Metaitus.Interfaces
{
    public interface ITriggerHandler
    {
        void Triggered(MTrigger triggered, MTrigger triggerer);
    }
}
