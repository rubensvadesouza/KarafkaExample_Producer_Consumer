using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberProducerSync.EF
{
    public interface IEntityBase
    {
        string ID { get; set; }
    }
}
