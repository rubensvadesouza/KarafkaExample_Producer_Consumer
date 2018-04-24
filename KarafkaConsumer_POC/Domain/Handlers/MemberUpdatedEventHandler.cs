using System.Threading.Tasks;
using CQRS.MongoDB;
using KarafkaConsumer_POC.Contracts.Messages;
using KarafkaConsumer_POC.Domain.Aggregates;
using KarafkaConsumer_POC.Domain.Commands;
using KarafkaConsumer_POC.Domain.Events;
using KarafkaConsumer_POC.Domain.Queries;

namespace KarafkaConsumer_POC.Domain.Handlers
{
    public class MemberUpdatedEventHandler
    {
        public MemberUpdatedEventHandler(MemberUpdatedCommand command, MemberQueryReader reader)
        {
            _command = command;
            _reader = reader;
        }

        MemberUpdatedCommand _command;
        MemberQueryReader _reader;
        public async Task<bool> HandleMember(MemberUpdatedMessage message)
        {
            var e = new MemberUpdatedEvent(MongoUtils.GenerateNewObjectId(), message.LegacyID, message.FullName, message.Age, message.CellNumber, message.DateOfBirth, message.RequestId, message.RequestDate);
            var agg = _reader.ReadOneAsync(x => x.Member.LegacyID == message.LegacyID).Result;

            if (agg == null)
            {
                agg = MemberAggregate.New();
            }

            try
            {
                if (message.Version <= agg.Version && agg.HasEvent(e))
                {
                    return false;
                }

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
