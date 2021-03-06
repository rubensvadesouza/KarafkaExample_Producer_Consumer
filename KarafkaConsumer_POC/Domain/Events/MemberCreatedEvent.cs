﻿using System;
using EventSourcing.Events;

namespace KarafkaConsumer_POC.Domain.Events
{
    public class MemberCreatedEvent : Event, IMemberPersonalInfoEvent
    {
        public string ID { get; }
        public string FullName { get; }
        public long Age { get; }
        public string CellNumber { get; }
        public DateTime DateOfBirth { get; }
        public string LegacyID { get; }
        public string EventType { get; }

        public MemberCreatedEvent(string ID, string LegacyID, string FullName, long Age, string CellNumber, DateTime DateOfBirth, string RequestID, DateTime EventDate) : base(RequestID, EventDate)
        {
            this.ID = ID;
            this.LegacyID = LegacyID;
            this.FullName = FullName;
            this.Age = Age;
            this.CellNumber = CellNumber;
            this.DateOfBirth = DateOfBirth;
            EventType = MemberEvents.Create;
        }

    }
}
