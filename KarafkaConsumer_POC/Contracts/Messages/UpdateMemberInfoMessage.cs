using System;

namespace KarafkaConsumer_POC.Contracts.Messages
{
    public class UpdateMemberInfoMessage : BaseMessage
    {
        public string FullName { get; set; }
        public long Age { get; set; }
        public string CellNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
