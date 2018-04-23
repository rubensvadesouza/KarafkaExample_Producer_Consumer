using CQRS.MongoDB;
using KarafkaConsumer_POC.Domain.Aggregates;
using KarafkaConsumer_POC.Domain.Events;

namespace KarafkaConsumer_POC.Domain.Queries
{
    public class MemberQueryReader : QueryReader<MemberAggregate, IMemberPersonalInfoEvent>
    {
        public MemberQueryReader(MongoProvider provider) : base(provider) { }
    }
}
