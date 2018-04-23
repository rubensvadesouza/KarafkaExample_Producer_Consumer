using EventSourcing.Events;

namespace EventSourcing.Aggregates
{
    public abstract class AggregateRoot<TEvent> where TEvent : IEvent
    {
        public string Id { get; set; }
        private bool _delivered { get; set; }
        public bool Delivered
        {
            get
            {
                return _delivered;
            }
            set
            {
                if (value)
                {
                    Version++;
                }
                _delivered = value;
            }
        }
        public long Version { get; set; }

        public virtual void RebuildFromEventStream()
        {
        }

        public virtual void ApplyChange(TEvent @event)
        {
        }
    }
}
