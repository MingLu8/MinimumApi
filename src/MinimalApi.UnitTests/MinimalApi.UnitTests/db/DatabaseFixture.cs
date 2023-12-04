using Microsoft.Data.SqlClient;
using RepoDb;
using Xunit.Extensions.AssemblyFixture;

[assembly: TestFramework(AssemblyFixtureFramework.TypeName, AssemblyFixtureFramework.AssemblyName)]

namespace MinimalApi.UnitTests.db
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            Db = new SqlConnection("Server=(local);Database=minimalApi;user Id=sa;password=Y3arz00o@;TrustServerCertificate=True;");
            GlobalConfiguration
            .Setup()
            .UseSqlServer();
        }

        public void Dispose()
        {
            // ... clean up test data from the database ...
        }

        public SqlConnection Db { get; private set; }
    }
}
