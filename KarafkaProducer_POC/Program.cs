
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KarafkaProducer_POC
{
    class Program
    {
        protected static IConfigurationRoot _config { get; set; }
        public static void Main()
        {
            _config = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
         .Build();

            var config = GetConfig();

            using (var producer = new Producer<int, string>(config, new IntSerializer(), new StringSerializer(Encoding.UTF8)))
            {
                for (int i = 0; i < 10000; i++)
                {
                    producer.ProduceAsync("n733uiq5-default", i, $"teste de Envio de Mensagem {i}")
                   .ContinueWith(result =>
                   {
                       var msg = result.Result;
                       if (msg.Error.Code != ErrorCode.NoError)
                       {
                           Console.WriteLine($"failed to deliver message: {msg.Error.Reason}");
                       }
                       else
                       {
                           Console.WriteLine($"delivered to: {result.Result.TopicPartitionOffset}");
                       }
                   });
                }


                producer.Flush(TimeSpan.FromSeconds(10));
            }
        }

        private static Dictionary<string, object> GetConfig()
        {
            var config = new Dictionary<string, object>
            {
                { "bootstrap.servers",_config["Karafka:brokers"].ToString() },
                { "sasl.mechanisms", "SCRAM-SHA-256" },
                { "security.protocol", "SASL_SSL" },
                { "ssl.ca.location", _config["Karafka:caLocation"].ToString() },
                { "sasl.username", _config["Karafka:user"].ToString() },
                { "sasl.password", _config["Karafka:password"].ToString() },
                {"debug","all" }
            };

            return config;
        }
    }
}