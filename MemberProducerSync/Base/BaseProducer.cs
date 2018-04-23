using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using MemberProducerSync.Utils;
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
        public virtual async Task<ProducerResult> Send(string message)
        {
            return await Send(0, message);
        }

        public virtual async Task<ProducerResult> Send(int key, string message)
        {
            var task = Task.Run(() =>
            {
                using (var producer = new Producer<int, string>(GetConfig(), new IntSerializer(), new StringSerializer(Encoding.UTF8)))
                {
                    var ret = producer.ProduceAsync(ConfigHelper.Configuration.GetValue<string>("Karafka:topicName"), key, message).Result;

                    if (ret.Error.HasError)
                    {
                        return ProducerResult.GetError($"Error Code:{ret.Error.Code} Reason: {ret.Error.Reason}");
                    }

                    producer.Flush(TimeSpan.FromSeconds(10));
                }

                return ProducerResult.Sucess;
            });

            return await task;

        }
        private Dictionary<string, object> GetConfig()
        {
            var config = new Dictionary<string, object>
            {
                { "bootstrap.servers", ConfigHelper.Configuration.GetValue<string>("Karafka:brokers")},
                { "sasl.mechanisms", "SCRAM-SHA-256" },
                { "security.protocol", "SASL_SSL" },
                { "ssl.ca.location",ConfigHelper.Configuration.GetValue<string>("Karafka:caLocation")},
                { "sasl.username", ConfigHelper.Configuration.GetValue<string>("Karafka:user")},
                { "sasl.password",ConfigHelper.Configuration.GetValue<string>("Karafka:password") },
            };

            return config;
        }
    }
}
