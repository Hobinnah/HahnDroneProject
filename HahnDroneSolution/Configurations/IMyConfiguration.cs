using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HahnDroneAPI.Configurations
{
    public interface IMyConfiguration
    {
        int MemoryCacheTimeOut { get; }
        string DomainUrl { get; }
        string DomainClientUrl { get; }
        int DroneCount { get; }

    }
}
