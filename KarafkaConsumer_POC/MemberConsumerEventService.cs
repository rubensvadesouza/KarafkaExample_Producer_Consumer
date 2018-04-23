using CQRS.MongoDB;
using EventSourcing.Events;
using KarafkaConsumer_POC.Contracts.Messages;
using KarafkaConsumer_POC.Domain.Aggregates;
using KarafkaConsumer_POC.Domain.Commands;
using KarafkaConsumer_POC.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MemberConsumerSync
{
    public class MemberConsumerEventService
    {

        public void ProcessMessage(string message)
        {

            Mapper();
            var baseMessage = JsonConvert.DeserializeObject<BaseMessage>(message);

            switch (baseMessage.Code)
            {
                case MemberEvents.Create:
                    ProcessAddMessage(message);
                    break;

                case MemberEvents.Update:
                    ProcessUpdateMessage(message);
                    break;

                default:
                    break;
            }
        }

        private async Task<string> ProcessAddMessage(string message)
        {
            var member = JsonConvert.DeserializeObject<AddMemberEvent>(message);
            var cmd = new MemberCreateCommand(new MongoProvider());
            var agg = MemberAggregate.New();
            agg.ApplyChange(member);
            return await cmd.AddAsync(agg);
        }

        private async void ProcessUpdateMessage(string message)
        {
            var member = JsonConvert.DeserializeObject<UpdatePersonalInfoEvent>(message);
            var cmd = new UpdateMemberCommand(new MongoProvider());
            var agg = MemberAggregate.New();
            agg.ApplyChange(member);
            await cmd.UpdateAsync(agg);
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

            if (!BsonClassMap.IsClassMapRegistered(typeof(AddMemberEvent)))
            {
                BsonClassMap.RegisterClassMap<AddMemberEvent>(x =>
                {
                    x.AutoMap();
                    x.MapProperty(p => p.ID);
                    x.MapProperty(p => p.LegacyID);
                    x.MapProperty(p => p.Age);
                    x.MapProperty(p => p.CellNumber);
                    x.MapProperty(p => p.DateOfBirth);
                    x.MapProperty(p => p.EventType);
                    x.MapProperty(p => p.FullName);
                    x.MapCreator(m => new AddMemberEvent(m.ID, m.LegacyID, m.FullName, m.Age, m.CellNumber, m.DateOfBirth, m.RequestID, m.EventDate));
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(UpdatePersonalInfoEvent)))
            {
                BsonClassMap.RegisterClassMap<UpdatePersonalInfoEvent>(x =>
                {
                    x.AutoMap();
                    x.MapProperty(p => p.ID);
                    x.MapProperty(p => p.LegacyID);
                    x.MapProperty(p => p.Age);
                    x.MapProperty(p => p.CellNumber);
                    x.MapProperty(p => p.DateOfBirth);
                    x.MapProperty(p => p.EventType);
                    x.MapProperty(p => p.FullName);
                    x.MapCreator(m => new UpdatePersonalInfoEvent(m.ID, m.LegacyID, m.FullName, m.Age, m.CellNumber, m.DateOfBirth, m.RequestID, m.EventDate));
                });
            }
        }
    }
}
