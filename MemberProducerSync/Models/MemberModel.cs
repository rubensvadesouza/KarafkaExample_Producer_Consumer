using MemberProducerSync.Base;
using System;

namespace MemberProducerSync.Model
{
    public class MemberModel : BaseMongo
    {
        public string ID { get; set; }
        public int Source { get; set; }
        public string Code { get; set; }
        public string RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public string FullName { get; set; }
        public long Age { get; set; }
        public string CellNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime Date { get; set; }
        public long Version { get; set; }
    }
}