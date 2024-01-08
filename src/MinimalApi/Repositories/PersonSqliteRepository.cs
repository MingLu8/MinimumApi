using MinimumApi.Entities;
using System.Data;

namespace MinimumApi.Repositories
{
  public class PersonSqliteRepository(IDbConnection connection) : RepoDbSqlLiteRepository<Person>(connection), IPersonRepository
  {
  }
}
