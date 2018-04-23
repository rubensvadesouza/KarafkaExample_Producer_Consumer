using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using KarafkaConsumer_POC.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KarafkaConsumer_POC
{

    public class Program
    {

        protected static IConfigurationRoot _config { get; set; }
        public static void Main()
        {
            _config = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("appconfig.json", optional: true, reloadOnChange: true)
                         .Build();


            var config = GetConfig();

            using (var consumer = new Consumer<int, string>(config, new IntDeserializer(), new StringDeserializer(Encoding.UTF8)))
            {
                consumer.Subscribe(_config["topicName"]);

                //consumer.OnConsumeError += (_, err)
                //    => Console.WriteLine($"consume error: {err.Error.Reason}");

                consumer.OnMessage += ConsumeMessage;

                //consumer.OnPartitionEOF += (_, tpo)
                //    => Console.WriteLine($"end of partition: {tpo}");

                while (true)
                {
                    consumer.Poll(TimeSpan.FromMilliseconds(100));
                }
            }

        }

        public static void ConsumeMessage(object sender, Message<int, string> message)
        {

            var model = JsonConvert.DeserializeObject<MemberModel>(message.Value);


        }

        private static void SendRequest(MemberModel model)
        {
            //TODO: Disparar API
        }

        private static MemberModel InsertEvent(MemberModel model)
        {
            return null;
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
                { "group.id", Guid.NewGuid().ToString() },
                { "auto.offset.reset", "smallest" },
            };

            return config;
        }
    }


}
