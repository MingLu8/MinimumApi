using MinimumApi.Entities;
using MinimumApi.Services;
using System.Data.Common;
using System.Linq.Expressions;

namespace MinimumApi.Repositories
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAllPeopleAsync();
        Task<Person> GetPersonByIdAsync(long id);
        Task<IEnumerable<Person>> FindPersonAsync(Expression<Func<Person, bool>> where);
        Task<bool> ExistsAsync(long id);
        Task AddPersonAsync(Person person);
        Task UpdatePersonAsync(Person person);
        Task DeletePersonAsync(long id);
    }
}
