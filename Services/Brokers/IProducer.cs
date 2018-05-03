using MemberProducerSync.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberProducerSync.Producers.Base
{
    public interface IProducer
    {
        ProducerResult Send(int key, string message);

        Task<ProducerResult> SendAsync(int key, string message);
    }
}
