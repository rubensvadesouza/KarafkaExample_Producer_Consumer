using System.Threading.Tasks;
using EventSourcing.Aggregates;
using EventSourcing.Events;
using MongoDB.Driver;

namespace CQRS.MongoDB
{
    public abstract class UpdateCommand<TAggregate, TEvent> : IUpdateCommand<TAggregate, TEvent> where TAggregate : AggregateRoot<TEvent>, new() where TEvent : IEvent
    {
        private MongoProvider _provider;

        public UpdateCommand(MongoProvider provider)
        {
            _provider = provider;
        }
        public async Task UpdateAsync(TAggregate aggregate)
        {
            await _provider.Collection<TAggregate, TEvent>().ReplaceOneAsync(x => x.Id == aggregate.Id, aggregate);
        }
    }
}
