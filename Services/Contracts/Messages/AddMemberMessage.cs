using System;
using System.Collections.Generic;

namespace KarafkaConsumer_POC.Contracts.Messages
{
    public class AddMemberMessage : BaseMessage
    {
        public string FullName { get; set; }
        public long Age { get; set; }
        public string CellNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<AddressModel> Addresses { get; set; }
    }

    public class AddressModel
    {
        public string StreetName { get; set; }
        public int StreetNumber { get; set; }
        public string ReferencePoint { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public bool DefaultAddress { get; set; }
        public string Type { get; set; }
    }
}
