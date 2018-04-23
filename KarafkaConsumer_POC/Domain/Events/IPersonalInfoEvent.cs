using System;
using EventSourcing.Events;

namespace KarafkaConsumer_POC.Domain.Events
{
    public interface IPersonalInfoEvent : IEvent
    {
        string ID { get; }
        string LegacyID { get; }
        string FullName { get; }
        long Age { get; }
        string CellNumber { get; }
        DateTime DateOfBirth { get; }
        string EventType { get; }
    }
}
