using MongoDB.Bson;
using System;

namespace MemberProducerSync.Model
{
    public class MemberModel
    {
        public ObjectId _id { get; set; }
        public string Id { get; set; }
        public string Code { get; set; }
        public string RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public string FullName { get; set; }
        public long Age { get; set; }
        public string CellNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime Date { get; set; }
    }
}