using System;

namespace KarafkaConsumer_POC.Contracts.Messages
{
    public class BaseMessage : IMessage
    {
        public string LegacyID { get; set; }
        public string Code { get; set; }        
        public string RequestId { get; set; }
        public DateTime RequestDate { get; set; }

        public long Version { get; set; }
    }
}

