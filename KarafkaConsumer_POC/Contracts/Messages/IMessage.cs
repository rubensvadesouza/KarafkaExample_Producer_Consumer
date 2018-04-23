using System;

namespace KarafkaConsumer_POC.Contracts.Messages
{
    public interface IMessage
    {
        string LegacyID { get; set; }
        string Code { get; set; }
        string RequestId { get; set; }
        DateTime RequestDate { get; set; }
    }
}
