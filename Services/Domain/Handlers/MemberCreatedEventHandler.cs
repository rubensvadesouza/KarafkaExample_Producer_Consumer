using KarafkaConsumer_POC.Contracts.Messages;
using KarafkaConsumer_POC.Domain.Aggregates;
using KarafkaConsumer_POC.Domain.Commands;
using KarafkaConsumer_POC.Domain.Events;
using KarafkaConsumer_POC.Domain.Queries;
using System;
using System.Threading.Tasks;

namespace KarafkaConsumer_POC.Domain.Handlers
{
    public class MemberCreatedEventHandler
    {
        public MemberCreatedEventHandler(MemberCreatedCommand command, MemberQueryReader qReader)
        {
            _command = command;
            _reader = qReader;
        }

        private MemberCreatedCommand _command;
        private MemberQueryReader _reader;

        public async Task<bool> HandleMember(AddMemberMessage message)
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
                            && x.ID == message.ID
                            && x.FullName == message.FullName
                            && x.Age == message.Age
                            && x.CellNumber == message.CellNumber
                            && x.DateOfBirth == message.DateOfBirth);

            if (naoProcessa)
            {
                return false;
            }

            try
            {
                var e = new MemberCreatedEvent(message.ID, message.LegacyID, message.FullName, message.Age, message.CellNumber, message.DateOfBirth, message.RequestId, message.RequestDate);
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