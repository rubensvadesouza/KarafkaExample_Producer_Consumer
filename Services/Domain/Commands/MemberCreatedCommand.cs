using CQRS.MongoDB;
using KarafkaConsumer_POC.Domain.Aggregates;
using KarafkaConsumer_POC.Domain.Events;

namespace KarafkaConsumer_POC.Domain.Commands
{
    public class MemberCreatedCommand : CreateCommand<MemberAggregate, IMemberPersonalInfoEvent>
    {
        public MemberCreatedCommand(MongoProvider provider) : base(provider) { }
    }
}
