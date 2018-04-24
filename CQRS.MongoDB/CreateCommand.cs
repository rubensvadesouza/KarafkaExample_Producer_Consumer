using EventSourcing.Aggregates;
using EventSourcing.Events;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace CQRS.MongoDB
{
    public abstract class CreateCommand<TAggregate, TEvent> : ICreateCommand<TAggregate, TEvent> where TAggregate : AggregateRoot<TEvent>, new() where TEvent : IEvent
    {
        private MongoProvider _provider;

        public CreateCommand(MongoProvider provider)
        {
            _provider = provider;
        }

        public async virtual Task<string> AddAsync(TAggregate aggregate)
        {
            try
            {
                aggregate.Id = ObjectId.GenerateNewId().ToString();
                await _provider.Collection<TAggregate, TEvent>().InsertOneAsync(aggregate);
                return aggregate.Id.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}