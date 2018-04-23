using CQRS.MongoDB;
using KarafkaConsumer_POC.Domain.Aggregates;
using KarafkaConsumer_POC.Domain.Events;

namespace KarafkaConsumer_POC.Domain.Commands
{
    public class MemberUpdatedCommand : UpdateCommand<MemberAggregate, IMemberPersonalInfoEvent>
    {
        public MemberUpdatedCommand(MongoProvider provider) : base(provider) { }
    }
}
