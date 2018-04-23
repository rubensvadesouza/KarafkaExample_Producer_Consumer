using System.Threading.Tasks;
using EventSourcing.Aggregates;
using EventSourcing.Events;

namespace CQRS
{
    public interface IUpdateCommand<TAggregate, TEvent> where TAggregate : AggregateRoot<TEvent>, new() where TEvent : IEvent
    {
        Task UpdateAsync(TAggregate aggregate);
    }
}
