using MemberProducerSync.EF;
using MemberProducerSync.EF.Models;
using MemberProducerSync.Repository.Base;
using MemberProducerSync.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberProducerSync.Repository
{
    public class MemberEntityRepository : EntityBaseRepository<MemberEntity>, IMemberEntityRepository
    {

        public MemberEntityRepository(MemberContext context)
            : base(context)
        {
        }

    }
}
