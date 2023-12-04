using MinimumApi.Entities;
using MinimumApi.Services;
using System.Data.Common;
using System.Linq.Expressions;

namespace MinimumApi.Repositories
{
    public interface IPersonRepository : IGenericRepository<Person>
    {             
    }
}
