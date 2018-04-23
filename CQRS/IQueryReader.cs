using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventSourcing.Aggregates;
using EventSourcing.Events;

namespace CQRS
{
    public interface IQueryReader<TAggregate, TEvent> where TAggregate : AggregateRoot<TEvent>, new() where TEvent : IEvent
    {
        Task<TAggregate> ReadOneAsync(string id);
        Task<TAggregate> ReadOneAsync(Expression<Func<TAggregate, bool>> predicate);
    }
}
