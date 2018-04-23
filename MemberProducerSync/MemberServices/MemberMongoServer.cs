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


        public void InsertMember(MemberModel model)
        {
            IMongoDatabase db = _client.GetDatabase("Member");
            var collection = db.GetCollection<MemberModel>("Members");

            model.GeneratorDate = DateTime.Now;

            var member = Find(model.ID);

            if (member != null)
            {
                model.EventType = MemberEvents.Create;
                Update(member);
            }
            else
            {
                model.EventType = MemberEvents.Create;
                collection.InsertOne(model);
            }

            HttpHelper.SendEventMember(model);
        }

        public MemberModel Find(string id)
        {
            IMongoDatabase db = _client.GetDatabase("Member");

            var collection = db.GetCollection<MemberModel>("Members");
            return collection.Find(x => x.ID == id).FirstOrDefault();
        }

        public void Update(MemberModel member)
        {
            IMongoDatabase db = _client.GetDatabase("Member");
            var collection = db.GetCollection<MemberModel>("Members");

            var filter = Builders<MemberModel>.Filter.Eq(s => s.ID, member.ID);
            var result = collection.ReplaceOneAsync(filter, member).GetAwaiter();
        }

    }
}
