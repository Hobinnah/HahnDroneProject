using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HahnDroneAPI.Configurations
{
    public class MyConfiguration : IMyConfiguration
    {
        public IConfiguration configuration { get; }
        public MyConfiguration(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public int MemoryCacheTimeOut
        {
            get
            {
                return configuration.GetValue<int>("MemoryCache:CacheTimeOutInHours");
            }
        }

        public string DomainUrl
        {
            get
            {
                return configuration["Domain:domainUrl"].ToString();
            }
        }

        public string DomainClientUrl
        {
            get
            {
                return configuration["Domain:clientUrl"].ToString();
            }
        }

        public int DroneCount
        {
            get
            {
                return Convert.ToInt16(configuration["DroneCount"].ToString());
            }
        }

    }
}
