using System;

namespace EventSourcing.Events
{
    public abstract class Event : IEvent
    {
        public string RequestID { get; }
        public DateTime EventDate { get; }

        public Event(string RequestID, DateTime EventDate)
        {
            this.RequestID = RequestID;
            this.EventDate = EventDate;
        }
    }
}
