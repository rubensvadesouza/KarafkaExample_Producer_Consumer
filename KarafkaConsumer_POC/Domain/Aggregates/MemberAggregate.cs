using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EventSourcing.Aggregates;
using KarafkaConsumer_POC.Domain.Entities;
using KarafkaConsumer_POC.Domain.Events;

namespace KarafkaConsumer_POC.Domain.Aggregates
{
    public class MemberAggregate : AggregateRoot<IMemberPersonalInfoEvent>
    {
        public Member Member { get; private set; }

        public override void RebuildEventStream()
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

        public override void AddEventToStream(IMemberPersonalInfoEvent @event)
        {
            Events.Add(@event);
        }

        //Checks if an event already exists in the stream
        public override bool HasEvent(Func<IMemberPersonalInfoEvent, bool> predicate)
        {
            //TODO: Implement verification by Event Type
            if (Events.Any(predicate))
            {
                return true;
            }
            return false;
        }

        public static MemberAggregate New() => new MemberAggregate { Events = new List<IMemberPersonalInfoEvent>() };
    }
}