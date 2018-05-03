using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MemberProducerSync.Utils
{
    public static class ConfigHelper
    {
        public static IConfigurationRoot Configuration => GetConfig();

        private static IConfigurationRoot GetConfig()
        {
            return new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appconfig.json", optional: true, reloadOnChange: true)
             .Build();
        }
    }
}
