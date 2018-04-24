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
            var agg = _reader.ReadOneAsync(x => x.Member.LegacyID == message.LegacyID).Result;

            if (agg == null)
            {
                agg = MemberAggregate.New();
            }

            try
            {
                var ID = MongoUtils.GenerateNewObjectId();
                agg.ApplyChange(new MemberUpdatedEvent(ID, message.LegacyID, message.FullName, message.Age, message.CellNumber, message.DateOfBirth, message.RequestId, message.RequestDate));
                agg.RebuildFromEventStream();
                await _command.UpdateAsync(agg);
            }
            catch
            {
                //TODO: Handle Exceptions
                return false;
            }
            return true;
        }
    }
}