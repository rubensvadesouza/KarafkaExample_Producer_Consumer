using Dharma.Configurations;
using Dharma.Data.CassandraDB.Repositories;
using MemberStressTest.Entities;

namespace MemberStressTest.Repositories
{
    public class MemberCassandraRepository : CassandraRepository<MemberCassandraEntity>
    {
        public MemberCassandraRepository(CassandraOptions configuration)
            : base(configuration)
        {
        }
    }
}