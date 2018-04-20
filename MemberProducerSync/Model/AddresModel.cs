using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberProducerSync.Model
{
    public class AddressModel
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
