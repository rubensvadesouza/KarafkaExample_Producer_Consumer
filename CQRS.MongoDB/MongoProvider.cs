using EventSourcing.Aggregates;
using EventSourcing.Events;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace CQRS.MongoDB
{
    public class MongoProvider
    {
        private readonly IMongoDatabase _database;
        public IMongoCollection<TAggregate> Collection<TAggregate, TEvent>() where TAggregate : AggregateRoot<TEvent>, new() where TEvent : IEvent
        {
            return _database.GetCollection<TAggregate>(typeof(TAggregate).Name);
        }

        public IMongoQueryable<TAggregate> QueryCollection<TAggregate, TEvent>() where TAggregate : AggregateRoot<TEvent>, new() where TEvent : IEvent
        {
            return _database.GetCollection<TAggregate>(typeof(TAggregate).Name).AsQueryable();
        }
        public MongoProvider() => _database = GetDatabase("mongodb://localhost:27017", "membersEventStore");
        private IMongoDatabase GetDatabase(string connString, string dbName) => new MongoClient(connString)?.GetDatabase(dbName);
    }
}
