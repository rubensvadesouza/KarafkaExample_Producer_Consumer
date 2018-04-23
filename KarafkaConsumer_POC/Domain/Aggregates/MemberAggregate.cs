using System.Collections.Generic;
using System.Linq;
using EventSourcing.Aggregates;
using KarafkaConsumer_POC.Domain.Entities;
using KarafkaConsumer_POC.Domain.Events;

namespace KarafkaConsumer_POC.Domain.Aggregates
{
    public class MemberAggregate : AggregateRoot<IMemberPersonalInfoEvent>
    {
        internal List<IMemberPersonalInfoEvent> Events { get; set; }
        internal Member Member { get; private set; }
        public override void RebuildFromEventStream()
        {
            Member = new Member();
            Events.OrderBy(x => x.EventDate).ToList().ForEach(x =>
            {
                Member.ID = x.ID;
                Member.LegacyID = x.LegacyID;
                Member.Age = x.Age;
                Member.CellNumber = x.CellNumber;
                Member.DateOfBirth = x.DateOfBirth;
                Member.FullName = x.FullName;
            });
        }

        public override void ApplyChange(IMemberPersonalInfoEvent @event)
        {
            Events.Add(@event);
        }

        public static MemberAggregate New() => new MemberAggregate { Events = new List<IMemberPersonalInfoEvent>() };
    }
}
