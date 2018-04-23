using System.Threading.Tasks;
using EventSourcing.Aggregates;
using EventSourcing.Events;

namespace CQRS
{
    public interface ICreateCommand<TAggregate, TEvent> where TAggregate : AggregateRoot<TEvent>, new() where TEvent : IEvent
    {
        Task<string> AddAsync(TAggregate aggregate);
    }
}
