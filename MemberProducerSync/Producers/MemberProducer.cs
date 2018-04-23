using MemberProducerSync.Producer.Base;
using MemberProducerSync.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberProducerSync.Producer
{
    public class MemberProducer : BaseProducer
    {
        public override Task<ProducerResult> Send(string message)
        {
            return base.Send(message);
        }
    }
}
