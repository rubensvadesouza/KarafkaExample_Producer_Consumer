using CQRS.MongoDB;
using KarafkaConsumer_POC.Domain.Aggregates;
using KarafkaConsumer_POC.Domain.Events;

namespace KarafkaConsumer_POC.Domain.Commands
{
    public class ReadMemberCommand : ReadCommand<MemberAggregate, IMemberPersonalInfoEvent>
    {
        public ReadMemberCommand(MongoProvider provider) : base(provider) { }
    }
}
