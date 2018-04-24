using System.Threading.Tasks;
using EventSourcing.Events;
using KarafkaConsumer_POC.Contracts.Messages;
using KarafkaConsumer_POC.Domain.Events;
using KarafkaConsumer_POC.Domain.Handlers;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;

namespace MemberConsumerSync
{
    public class MemberConsumerEventService
    {
        MemberCreatedEventHandler _createdHandler;
        MemberUpdatedEventHandler _updatedHandler;

        public MemberConsumerEventService(MemberCreatedEventHandler created, MemberUpdatedEventHandler updated)
        {
            _createdHandler = created;
            _updatedHandler = updated;
        }

        public async void ProcessMessage(string message)
        {
            var baseMessage = JsonConvert.DeserializeObject<BaseMessage>(message);

            switch (baseMessage.Code)
            {
                case MemberEvents.Create:
                    await ProcessAddMessage(message);
                    break;

                case MemberEvents.Update:
                    await ProcessUpdateMessage(message);
                    break;

                default:
                    break;
            }
        }

        private async Task ProcessAddMessage(string message)
        {
            var msg = JsonConvert.DeserializeObject<AddMemberMessage>(message);
            var ret = await _createdHandler.HandleMember(msg);

            if (ret)
            {
                //Do Stuff, map models and call other services
            }
        }

        private async Task ProcessUpdateMessage(string message)
        {
            var msg = JsonConvert.DeserializeObject<MemberUpdatedMessage>(message);
            var ret = await _updatedHandler.HandleMember(msg);

            if (ret)
            {
                //Do Stuff, map models and call other services
            }
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
        }
    }
}