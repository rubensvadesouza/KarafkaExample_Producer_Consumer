using System.Collections.Generic;
using System.Linq;
using EventSourcing.Aggregates;
using KarafkaConsumer_POC.Domain.Entities;
using KarafkaConsumer_POC.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

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
                if (!string.IsNullOrWhiteSpace(x.ID)) { Member.ID = x.ID; }
                if (!string.IsNullOrWhiteSpace(x.ID)) { Member.LegacyID = x.LegacyID; }
                if (!string.IsNullOrWhiteSpace(x.ID)) { Member.CellNumber = x.CellNumber; }
                if (!string.IsNullOrWhiteSpace(x.FullName)) { Member.FullName = x.FullName; }
                if (x.Age > 0) { Member.Age = x.Age; }
                if (x.DateOfBirth != null || x.DateOfBirth != DateTime.MinValue) { Member.DateOfBirth = x.DateOfBirth; }
            });
        }

        public override void ApplyChange(IMemberPersonalInfoEvent @event)
        {
            Events.Add(@event);
        }

        public static MemberAggregate New() => new MemberAggregate { Events = new List<IMemberPersonalInfoEvent>() };
    }
}