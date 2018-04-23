using MemberProducerSync.EF.Models;
using MemberProducerSync.Model;
using MemberProducerSync.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberProducerSync.Repository
{
    public interface IMemberRepository : IEntityBaseRepository<MemberEntity> { }
}
