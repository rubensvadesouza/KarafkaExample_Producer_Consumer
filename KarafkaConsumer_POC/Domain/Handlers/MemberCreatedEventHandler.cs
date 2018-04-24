using System;
using System.Threading.Tasks;
using CQRS.MongoDB;
using KarafkaConsumer_POC.Contracts.Messages;
using KarafkaConsumer_POC.Domain.Aggregates;
using KarafkaConsumer_POC.Domain.Commands;
using KarafkaConsumer_POC.Domain.Events;
using KarafkaConsumer_POC.Domain.Queries;

namespace KarafkaConsumer_POC.Domain.Handlers
{
    public class MemberCreatedEventHandler
    {
        public MemberCreatedEventHandler(MemberCreatedCommand command, MemberQueryReader qReader)
        {
            _command = command;
            _reader = qReader;
        }

        MemberCreatedCommand _command;
        MemberQueryReader _reader;

        public async Task<bool> HandleMember(AddMemberMessage message)
        {
            var e = new MemberCreatedEvent(MongoUtils.GenerateNewObjectId(), message.LegacyID, message.FullName, message.Age, message.CellNumber, message.DateOfBirth, message.RequestId, message.RequestDate);
            var agg = _reader.ReadOneAsync(x => x.Member.LegacyID == message.LegacyID).Result;

            if (agg != null && message.Version <= agg.Version
            && agg.HasEvent(e))
            {
                return true;
            }
            else if (agg == null)
            {
                agg = MemberAggregate.New();
            }

            try
            {
                agg.AddEventToStream(e);
                agg.RebuildEventStream();
                await _command.AddAsync(agg);
            }
            catch (Exception)
            {
                //TODO: Handle Exceptions
                return false;
            }

            return true;
        }
    }
}
