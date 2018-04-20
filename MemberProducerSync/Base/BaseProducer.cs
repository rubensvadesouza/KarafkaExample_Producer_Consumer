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
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        }

        public virtual async Task<ProducerResult> Send(string message)
        {
            return await Send(0, message);
        }

        public virtual async Task<ProducerResult> Send(int key, string message)
        {
            var t = Task.Run(() =>
            {
                using (var producer = new Producer<int, string>(GetConfig(), new IntSerializer(), new StringSerializer(Encoding.UTF8)))
                {

                    var ret = producer.ProduceAsync(_config["Karafka:topicName"], key, message).Result;

                    if (ret.Error.HasError)
                    {
                        return ProducerResult.GetError($"Error Code:{ret.Error.Code} Reason: {ret.Error.Reason}");
                    }

                    producer.Flush(TimeSpan.FromSeconds(10));
                }

                return ProducerResult.Sucess;
            });

            return await t;

        }

        private Dictionary<string, object> GetConfig()
        {

            var valor = _config["Karafka:brokers"];
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
