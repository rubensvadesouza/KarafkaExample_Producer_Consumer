using MemberProducerSync.EF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberProducerSync.EF
{
    public class MemberContext : DbContext
    {
        public MemberContext()
        {

        }

        public MemberContext(DbContextOptions<MemberContext> options)
            : base(options)
        {
        }

        public DbSet<MemberEntity> Members { get; set; }
    }
}
