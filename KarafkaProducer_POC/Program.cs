
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
         .AddJsonFile("appconfig.json", optional: true, reloadOnChange: true)
         .Build();

            var config = GetConfig();

            using (var producer = new Producer<int, string>(config, new IntSerializer(), new StringSerializer(Encoding.UTF8)))
            {
                //producer.ProduceAsync("n733uiq5-default", null, "teste da porra do certificado maldito que nao sei por que nao funfa direito")
                //   .ContinueWith(result =>
                //   {
                //       var msg = result.Result;
                //       if (msg.Error.Code != ErrorCode.NoError)
                //       {
                //           Console.WriteLine($"failed to deliver message: {msg.Error.Reason}");
                //       }
                //       else
                //       {
                //           Console.WriteLine($"delivered to: {result.Result.TopicPartitionOffset}");
                //       }
                //   });

                //producer.Flush(TimeSpan.FromSeconds(10));

                var task_1 = new System.Threading.Tasks.Task(() =>
                {
                    for (int i = 0; i < 10000; i++)
                    {
                        var result = producer.ProduceAsync("n733uiq5-default", 1, $"Mensagem de Teste {i}").Result;
                    }
                });

                var task_2 = new System.Threading.Tasks.Task(() =>
                {
                    for (int i = 0; i < 10000; i++)
                    {
                        var result = producer.ProduceAsync("n733uiq5-default", 2, $"Mensagem de Teste {i}").Result;
                    }
                });

                var task_3 = new System.Threading.Tasks.Task(() =>
                {
                    for (int i = 0; i < 10000; i++)
                    {
                        var result = producer.ProduceAsync("n733uiq5-default", 3, $"Mensagem de Teste {i}").Result;
                    }
                });

                task_1.Start();
                task_2.Start();
                task_3.Start();

                while (true)
                {
                }

                producer.Flush(TimeSpan.FromSeconds(10));
            }
        }



        private static Dictionary<string, object> GetConfig()
        {
            var config = new Dictionary<string, object>
            {
                { "bootstrap.servers",_config["brokers"].ToString() },
                { "sasl.mechanisms", "SCRAM-SHA-256" },
                { "security.protocol", "SASL_SSL" },
                { "ssl.ca.location", _config["caLocation"].ToString() },
                { "sasl.username", _config["user"].ToString() },
                { "sasl.password", _config["password"].ToString() },
                {"debug","all" }
            };

            return config;
        }
    }
}