using Microsoft.Extensions.Configuration;
using System;

namespace HahnDroneAPI.Configurations
{
    public class CustomConfiguration : ICustomConfiguration
    {
        public IConfiguration configuration { get; }
        public CustomConfiguration(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private string GetProperty(string key) => configuration[key];
        public int MemoryCacheTimeOut() => Convert.ToInt16(GetProperty("MemoryCache:CacheTimeOutInHours"));              
        public int DroneCount() => Convert.ToInt16(GetProperty("Drone:DroneCount"));
        public int BatteryLowerLimit() => Convert.ToInt16(GetProperty("Drone:BattreyLowerLimit"));
        public int GetDroneUpperWeightLimit() =>  Convert.ToInt16(GetProperty("Drone:DroneUpperWeightLimit"));

    }
}
