using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using CQRS.MongoDB;
using KarafkaConsumer_POC.Domain.Commands;
using KarafkaConsumer_POC.Domain.Handlers;
using KarafkaConsumer_POC.Domain.Queries;
using MemberConsumerSync;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KarafkaConsumer_POC
{

    public class Program
    {
        protected static IConfigurationRoot _config { get; set; }
        protected static MemberConsumerEventService _eventService { get; set; }
        private static ServiceProvider _services { get; set; }

        public static void Main()
        {
            _config = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("appconfig.json", optional: true, reloadOnChange: true)
                         .Build();

            _services = Setup();

            var _cHandler = _services.GetService<MemberCreatedEventHandler>();
            var uHandler = _services.GetService<MemberUpdatedEventHandler>();

            _eventService = new MemberConsumerEventService(_cHandler, uHandler);


            var config = GetConfig();


            using (var consumer = new Consumer<int, string>(config, new IntDeserializer(), new StringDeserializer(Encoding.UTF8)))
            {
                consumer.Subscribe(_config["topicName"]);

                consumer.OnMessage += ConsumeMessage;

                consumer.OnPartitionEOF += (_, tpo)
                    => Console.WriteLine($"end of partition: {tpo}");

                while (true)
                {
                    consumer.Poll(TimeSpan.FromMilliseconds(100));
                }
            }
        }

        public static void ConsumeMessage(object sender, Message<int, string> message)
        {
            _eventService.ProcessMessage(message.Value);
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

        private static ServiceProvider Setup()
        {
            var serviceProvider = new ServiceCollection()
                                     .AddScoped<MongoProvider>()
                                     .AddScoped<MemberCreatedCommand>()
                                     .AddScoped<MemberUpdatedCommand>()
                                     .AddScoped<MemberQueryReader>();

            return serviceProvider.BuildServiceProvider();
        }   
    }


}
