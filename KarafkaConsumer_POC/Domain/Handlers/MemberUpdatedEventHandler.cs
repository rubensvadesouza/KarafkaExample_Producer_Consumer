using CQRS.MongoDB;
using KarafkaConsumer_POC.Contracts.Messages;
using KarafkaConsumer_POC.Domain.Aggregates;
using KarafkaConsumer_POC.Domain.Commands;
using KarafkaConsumer_POC.Domain.Events;
using KarafkaConsumer_POC.Domain.Queries;
using System.Threading.Tasks;

namespace KarafkaConsumer_POC.Domain.Handlers
{
    public class MemberUpdatedEventHandler
    {
        public MemberUpdatedEventHandler(MemberUpdatedCommand command, MemberQueryReader reader)
        {
            _command = command;
            _reader = reader;
        }

        private MemberUpdatedCommand _command;
        private MemberQueryReader _reader;

        public async Task<bool> HandleMember(MemberUpdatedMessage message)
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
                var e = new MemberUpdatedEvent(MongoUtils.GenerateNewObjectId(), message.LegacyID, message.FullName, message.Age, message.CellNumber, message.DateOfBirth, message.RequestId, message.RequestDate);
                agg.AddEventToStream(e);
                agg.RebuildEventStream();
                agg.CommitChanges();
                await _command.UpdateAsync(agg);
            }
            catch
            {
                //TODO: Implement retry policy
                return false;
            }
            return true;
        }
    }
}