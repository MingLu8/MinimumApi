using MinimumApi.Repositories;
using System.Data;

namespace MinimalApi.UnitTests.db
{
    public class TestDataRepoDbRepository : RepoDbGenericRepository<TestData>
    {
        public TestDataRepoDbRepository(IDbConnection connection) : base(connection)
        {
        }
    }
}
