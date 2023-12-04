using MinimumApi.Entities;
using System.Data;

namespace MinimumApi.Repositories
{
    public class PersonRepoDbRepository : RepoDbGenericRepository<Person>, IPersonRepository
    {
        public PersonRepoDbRepository(IDbConnection connection) : base(connection)
        {
        }
    }
}
