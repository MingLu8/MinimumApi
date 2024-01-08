using MinimumApi.Entities;
using System.Data;

namespace MinimumApi.Repositories
{
  public class PersonSqlServerRepository(IDbConnection connection) : RepoDbMsSqlRepository<Person>(connection), IPersonRepository
  {
  }
}
