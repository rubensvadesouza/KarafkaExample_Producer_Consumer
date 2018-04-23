using System;
using System.Collections.Generic;

namespace KarafkaConsumer_POC.Domain.Entities
{
    internal class Member
    {
        public string ID { get; set; }
        public string LegacyID { get; set; }
        public string FullName { get; set; }
        public long Age { get; set; }
        public string CellNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<Address> Addresses { get; set; }

        public Member() { }
    }

    internal class Address
    {
        public string ID { get; set; }
        public string MemberID { get; set; }
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
