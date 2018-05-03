using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using CQRS.MongoDB;
using EventSourcing.Events;
using KarafkaConsumer_POC.Domain.Commands;
using KarafkaConsumer_POC.Domain.Events;
using KarafkaConsumer_POC.Domain.Handlers;
using KarafkaConsumer_POC.Domain.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MemberConsumerSync
{
    public class MemberConsumer
    {
        protected static IConfigurationRoot _config { get; set; }
        protected static MemberConsumerEventService _eventService { get; set; }
        private static ServiceProvider _services { get; set; }

        public void Process()
        {
            _config = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appconfig.json", optional: true, reloadOnChange: true)
                       .Build();
            Mapper();

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

        private static void Mapper()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Event)))
            {
                BsonClassMap.RegisterClassMap<Event>(x =>
                {
                    x.AutoMap();
                    x.MapProperty(p => p.RequestID);
                    x.MapProperty(p => p.EventDate);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(MemberCreatedEvent)))
            {
                BsonClassMap.RegisterClassMap<MemberCreatedEvent>(x =>
                {
                    x.AutoMap();
                    x.MapProperty(p => p.ID);
                    x.MapProperty(p => p.LegacyID);
                    x.MapProperty(p => p.Age);
                    x.MapProperty(p => p.CellNumber);
                    x.MapProperty(p => p.DateOfBirth);
                    x.MapProperty(p => p.EventType);
                    x.MapProperty(p => p.FullName);
                    x.MapCreator(m => new MemberCreatedEvent(m.ID, m.LegacyID, m.FullName, m.Age, m.CellNumber, m.DateOfBirth, m.RequestID, m.EventDate));
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(MemberUpdatedEvent)))
            {
                BsonClassMap.RegisterClassMap<MemberUpdatedEvent>(x =>
                {
                    x.AutoMap();
                    x.MapProperty(p => p.ID);
                    x.MapProperty(p => p.LegacyID);
                    x.MapProperty(p => p.Age);
                    x.MapProperty(p => p.CellNumber);
                    x.MapProperty(p => p.DateOfBirth);
                    x.MapProperty(p => p.EventType);
                    x.MapProperty(p => p.FullName);
                    x.MapCreator(m => new MemberUpdatedEvent(m.ID, m.LegacyID, m.FullName, m.Age, m.CellNumber, m.DateOfBirth, m.RequestID, m.EventDate));
                });
            }

            if (!BsonSerializer.IsTypeDiscriminated(typeof(DateTimeSerializer)))
            {
                BsonSerializer.RegisterSerializer(typeof(DateTime), new DateTimeSerializer(DateTimeKind.Local));
            }
        }

        private static ServiceProvider Setup()
        {
            var serviceProvider = new ServiceCollection()
                                     .AddScoped<MongoProvider>()
                                     .AddScoped<MemberCreatedEventHandler>()
                                     .AddScoped<MemberUpdatedEventHandler>()
                                     .AddScoped<MemberCreatedCommand>()
                                     .AddScoped<MemberUpdatedCommand>()
                                     .AddScoped<MemberQueryReader>();

            return serviceProvider.BuildServiceProvider();
        }
    }
}