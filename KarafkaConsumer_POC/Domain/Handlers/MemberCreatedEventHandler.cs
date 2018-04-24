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

            var agg = _reader.ReadOneAsync(x => x.Member.LegacyID == message.LegacyID)?.Result ?? MemberAggregate.New();

            if (agg != null && message.Version <= agg.Version
            && agg.HasEvent(x => x.LegacyID == message.LegacyID
            && x.FullName == message.FullName
            && x.Age == message.Age
            && x.CellNumber == message.CellNumber
            && x.DateOfBirth == message.DateOfBirth))
            {
                return false;
            }

            try
            {
                var e = new MemberCreatedEvent(MongoUtils.GenerateNewObjectId(), message.LegacyID, message.FullName, message.Age, message.CellNumber, message.DateOfBirth, message.RequestId, message.RequestDate);
                agg.AddEventToStream(e);
                agg.RebuildEventStream();
                agg.CommitChanges();
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
