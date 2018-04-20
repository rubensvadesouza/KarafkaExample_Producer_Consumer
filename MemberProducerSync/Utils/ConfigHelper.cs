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

        public static IConfigurationRoot GetConfig()
        {
            return new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .Build();
        }

        public static string GetField(string name)
        {
            return Configuration[name].ToString();
        }
    }
}
