using System;

namespace KarafkaConsumer_POC.Contracts.Messages
{
    public interface IMessage
    {
        string ID { get; set; }
        string LegacyID { get; set; }
        int Source { get; set; }
        string Code { get; set; }
        string RequestId { get; set; }
        long Version { get; set; }
        DateTime RequestDate { get; set; }
    }
}
