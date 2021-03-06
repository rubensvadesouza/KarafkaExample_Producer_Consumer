﻿using Microsoft.Extensions.Configuration;
using System.IO;

namespace MemberProducerSync.Utils
{
    public static class ConfigHelper
    {
        public static IConfigurationRoot Configuration => GetConfig();

        private static IConfigurationRoot GetConfig()
        {
            return new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .Build();
        }
    }
}