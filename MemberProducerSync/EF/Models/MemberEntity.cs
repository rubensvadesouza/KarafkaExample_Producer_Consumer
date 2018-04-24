using MemberProducerSync.EF.Interface;
using System;

namespace MemberProducerSync.EF.Models
{
    public class MemberEntity : IEntityBase
    {
        public string FullName { get; set; }
        public long Age { get; set; }
        public string CellNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ID { get; set; }

    }
}