using System;

namespace KarafkaConsumer_POC.Model
{
    public class MemberModel
    {
        public string ID { get; set; }
        public string Code { get; set; }
        public string RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public string FullName { get; set; }
        public long Age { get; set; }
        public string CellNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime Date { get; set; }

        public long Version { get; set; }
        //public List<AddressModel> Addresses { get; set; }
    }
}