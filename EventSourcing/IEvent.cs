using System;

namespace EventSourcing.Events
{
    public interface IEvent
    {
        string RequestID { get; }
        DateTime EventDate { get; }
    }
}
