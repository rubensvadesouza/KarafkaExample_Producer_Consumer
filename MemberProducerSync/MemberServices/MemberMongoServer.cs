using MemberProducerSync.Model;
using MemberProducerSync.Producer.Base;
using MemberProducerSync.Producers;
using MemberProducerSync.Utils;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MemberProducerSync.MemberService
{
    public class MemberMongoService
    {
        private MongoClient _client => new MongoClient(ConfigHelper.Configuration.GetValue<string>("MongoDB:connectionString"));

        private MemberProducer _sync => new MemberProducer(new ConfluentProducer());

        public async void InsertMember(MemberModel model)
        {
            IMongoDatabase db = _client.GetDatabase("Member");
            var collection = db.GetCollection<MemberModel>("Members");

            model.Date = DateTime.Now;

            var member = Find(model.ID);

            if (member != null)
            {
                model.Code = MemberEvents.Update;
                Update(model);
            }
            else
            {
                model.Code = MemberEvents.Create;
                await collection.InsertOneAsync(model);
            }

            _sync.Send(model);
        }

        public async Task<MemberModel> FindAsync(string id)
        {
            IMongoDatabase db = _client.GetDatabase("Member");

            var collection = db.GetCollection<MemberModel>("Members");
            return await collection.Find(x => x.ID == id).FirstOrDefaultAsync();
        }

        public MemberModel Find(string id)
        {
            IMongoDatabase db = _client.GetDatabase("Member");

            var collection = db.GetCollection<MemberModel>("Members");
            return collection.Find(x => x.ID == id).FirstOrDefault();
        }

        public async void Update(MemberModel member)
        {
            IMongoDatabase db = _client.GetDatabase("Member");
            var collection = db.GetCollection<MemberModel>("Members");

            await collection.ReplaceOneAsync(x => x.ID == member.ID, member);
        }
    }
}