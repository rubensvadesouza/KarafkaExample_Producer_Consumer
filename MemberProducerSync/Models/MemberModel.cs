using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberProducerSync.Model
{
    public class MemberModel
    {
        public ObjectId _id { get; set; }
        public string ID { get; set; }
        public string FullName { get; set; }
        public long Age { get; set; }
        public string CellNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }

    }
}
