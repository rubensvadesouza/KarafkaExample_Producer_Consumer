using System.Collections.Generic;
using System.Linq;
using EventSourcing.Events;

namespace EventSourcing.Aggregates
{
    public abstract class AggregateRoot<TEvent> where TEvent : IEvent
    {
        public string Id { get; set; }
        public virtual List<TEvent> Events { get; set; }
        private long _version { get; set; }
        public long Version { get { return _version; } private set { _version++; } }
        private bool _commited { get; set; }

        /// <summary>
        /// Rebuilds the aggregate based on the event stream
        /// </summary>
        public virtual void RebuildEventStream()
        {
        }


        public virtual void AddEventToStream(TEvent @event)
        {
        }

        /// <summary>
        /// This method increments the current Aggregate version plus one
        /// </summary>
        public void CommitChanges()
        {
            if (!_commited)
            {
                Version++;
                _commited = true;
            }            
        }

        public virtual bool HasEvent(TEvent e)
        {
            if (Events.Any(x => x.RequestID == e.RequestID
                             && x.EventDate == e.EventDate))
            {
                return true;
            }
            return false;
        }
    }
}
