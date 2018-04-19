using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberProducerSync.Base
{
    public class ProducerResult
    {
        public ProducerResult(EnumProducerResult result)
        {
            Result = result;
        }

        public EnumProducerResult Result { get; set; }

        public string Message { get; set; }

        public static ProducerResult Sucess = new ProducerResult(EnumProducerResult.Sucess);

        public static ProducerResult GetError(string error)
        {
            var result = new ProducerResult(EnumProducerResult.Error)
            {
                Message = error
            };

            return result;
        }

        public static ProducerResult GetWarning(string warning)
        {
            var result = new ProducerResult(EnumProducerResult.Error)
            {
                Message = warning
            };

            return result;
        }

    }
}

