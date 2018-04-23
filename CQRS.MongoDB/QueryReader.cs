using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventSourcing.Aggregates;
using EventSourcing.Events;
using MongoDB.Bson;

namespace CQRS.MongoDB
{
    public abstract class QueryReader<TAggregate, TEvent> : IQueryReader<TAggregate, TEvent> where TAggregate : AggregateRoot<TEvent>, new() where TEvent : IEvent
    {
        MongoProvider _provider;

        public QueryReader(MongoProvider provider)
        {
            _provider = provider;
        }
        public async Task<TAggregate> ReadOneAsync(string id)
        {
            var p = ObjectId.TryParse(id, out ObjectId i);
            //TODO: Refactor
            return await Task.Run(() => { return p ? _provider.QueryCollection<TAggregate, TEvent>().Where(x => x.Id == id).FirstOrDefault() : null; }).ConfigureAwait(true);
        }

        public async Task<TAggregate> ReadOneAsync(Expression<Func<TAggregate, bool>> predicate)
        {
            //TODO: Refactor
            return await Task.Run(() => { return predicate != null ? _provider.QueryCollection<TAggregate, TEvent>().FirstOrDefault(predicate) : null; }).ConfigureAwait(true);
        }
    }
}
