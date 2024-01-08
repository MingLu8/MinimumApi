using MinimumApi.Entities;
using System.Data;

namespace MinimumApi.Repositories
{
  public class PersonRepoDbRepository(IServiceProvider ioc) : RepoDbSqlLiteRepository<Person>(connection: ioc.GetKeyedService<IDbConnection>("sqlite")), IPersonRepository
  {
  }
}
