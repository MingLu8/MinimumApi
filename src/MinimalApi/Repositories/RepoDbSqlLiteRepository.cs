using System.Data;
using System.Data.SQLite;

namespace MinimumApi.Repositories
{
    public class RepoDbSqlLiteRepository<T>(IDbConnection connection) : RepoDbGenericRepository<T, SQLiteConnection>((SQLiteConnection)connection) where T : class
    {
    }
}
