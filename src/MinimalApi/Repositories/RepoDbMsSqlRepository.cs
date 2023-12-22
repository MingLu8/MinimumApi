using Microsoft.Data.SqlClient;
using System.Data;

namespace MinimumApi.Repositories
{
    public class RepoDbMsSqlRepository<T>(IDbConnection connection) : RepoDbGenericRepository<T, SqlConnection>((SqlConnection)connection) where T : class
    {
    }
}
