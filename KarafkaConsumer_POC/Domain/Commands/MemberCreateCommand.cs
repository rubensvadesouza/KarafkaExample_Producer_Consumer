using CQRS.MongoDB;
using KarafkaConsumer_POC.Domain.Aggregates;
using KarafkaConsumer_POC.Domain.Events;

namespace KarafkaConsumer_POC.Domain.Commands
{
    public class MemberCreateCommand : CreateCommand<MemberAggregate, IMemberPersonalInfoEvent>
    {
        public MemberCreateCommand(MongoProvider provider) : base(provider) { }
    }
}
