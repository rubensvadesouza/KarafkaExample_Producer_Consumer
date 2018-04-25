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
        public long Version { get; set; }

        public MemberUpdatedEventHandler(MemberUpdatedCommand command, MemberQueryReader reader)
        {
            _command = command;
            _reader = reader;
        }

        private MemberUpdatedCommand _command;
        private MemberQueryReader _reader;

        public async Task<bool> HandleMember(MemberUpdatedMessage message)
        {
            MemberAggregate agg;

            if (message.Source == 0)
            {
                agg = _reader.ReadOneAsync(x => x.Member.ID == message.ID)?.Result ?? MemberAggregate.New();
            }
            else if (message.Source == 1)
            {
                agg = _reader.ReadOneAsync(x => x.Member.LegacyID == message.LegacyID)?.Result ?? MemberAggregate.New();
            }
            else
            {
                agg = MemberAggregate.New();
            }

            bool naoProcessa = agg != null;
            naoProcessa &= message.Version <= agg.Version;
            naoProcessa &= agg.HasEvent(x => x.LegacyID == message.LegacyID
                            && x.FullName == message.FullName
                            && x.ID == message.ID
                            && x.Age == message.Age
                            && x.CellNumber == message.CellNumber
                            && x.DateOfBirth == message.DateOfBirth.ToLocalTime());

            if (naoProcessa)
            {
                return false;
            }

            try
            {
                var e = new MemberUpdatedEvent(message.ID, message.LegacyID, message.FullName, message.Age, message.CellNumber, message.DateOfBirth, message.RequestId, message.RequestDate);
                agg.AddEventToStream(e);
                agg.RebuildEventStream();
                agg.CommitChanges();
                Version = agg.Version;
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