using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberProducerSync.Base
{
    public class BaseProducer
    {
        protected readonly IConfigurationRoot _config;

        public BaseProducer()
        {
            _config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appconfig.json", optional: true, reloadOnChange: true)
            .Build();
        }

        public virtual ProducerResult Send(int key, string message)
        {
            using (var producer = new Producer<int, string>(GetConfig(), new IntSerializer(), new StringSerializer(Encoding.UTF8)))
            {
                for (int i = 0; i < 10000; i++)
                {
                    var ret = producer.ProduceAsync("n733uiq5-default", i, $"teste de Envio de Mensagem {i}").Result;

                    if (ret.Error.HasError)
                    {
                        return ProducerResult.GetError($"");
                    }
                }

                producer.Flush(TimeSpan.FromSeconds(10));
            }

            return ProducerResult.Sucess;
        }

        private Dictionary<string, object> GetConfig()
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
