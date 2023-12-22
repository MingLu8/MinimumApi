using MinimumApi.Repositories;
using System.Data;

namespace MinimalApi.UnitTests.db
{
    public class TestDataRepoDbRepository : RepoDbSqlLiteRepository<TestData>
    {
        public TestDataRepoDbRepository(IDbConnection connection) : base(connection)
        {
        }
    }
}
