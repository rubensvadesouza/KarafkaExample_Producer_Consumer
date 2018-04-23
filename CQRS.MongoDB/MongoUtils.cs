using MongoDB.Bson;

namespace CQRS.MongoDB
{
    public static class MongoUtils
    {
        public static string GenerateNewObjectId()
        {
            return ObjectId.GenerateNewId().ToString();
        }
    }
}
