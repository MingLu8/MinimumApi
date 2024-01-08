using MinimumApi.Entities;
using MinimumApi.Extensions;
using RepoDb;
using System.Data;
using System.Linq.Expressions;

namespace MinimumApi.Repositories
{
  public class PersonSqlServerRepository(IServiceProvider ioc) : RepoDbSqlLiteRepository<Person>(connection: ioc.GetKeyedService<IDbConnection>("sqlServer")), IPersonRepository
  {
  }
}
