using System;
using System.Threading.Tasks;
using CQRS.MongoDB;
using KarafkaConsumer_POC.Contracts.Messages;
using KarafkaConsumer_POC.Domain.Aggregates;
using KarafkaConsumer_POC.Domain.Commands;
using KarafkaConsumer_POC.Domain.Events;

namespace KarafkaConsumer_POC.Domain.Handlers
{
    public class MemberCreatedEventHandler
    {
        public MemberCreatedEventHandler(MemberCreateCommand command)
        {
            _command = command;
        }

        MemberCreateCommand _command;

        public async Task<bool> HandleMember(AddMemberMessage message)
        {
            var agg = MemberAggregate.New();

            try
            {
                var ID = MongoUtils.GenerateNewObjectId();
                agg.ApplyChange(new MemberCreatedEvent(ID, message.LegacyID, message.FullName, message.Age, message.CellNumber, message.DateOfBirth, message.RequestId, message.RequestDate));
                agg.RebuildFromEventStream();
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
