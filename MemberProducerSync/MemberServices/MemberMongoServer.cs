using MemberProducerSync.Model;
using MemberProducerSync.Utils;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MemberProducerSync.MemberService
{
    public class MemberMongoService
    {
        private MongoClient _client => new MongoClient(ConfigHelper.Configuration.GetValue<string>("MongoDB:connectionString"));


        public async void InsertMember(MemberModel model)
        {
            IMongoDatabase db = _client.GetDatabase("Member");
            var collection = db.GetCollection<MemberModel>("Members");

            model.GeneratorDate = DateTime.Now;

            var member = Find(model.ID);

            if (member != null)
            {
                model.EventType = MemberEvents.Create;
                model._id = member._id;
                Update(model);
            }
            else
            {
                model.EventType = MemberEvents.Update;
                await collection.InsertOneAsync(model);
            }

            HttpHelper.SendEventMember(model);
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
