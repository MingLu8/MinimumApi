using MinimumApi.Entities;
using MinimumApi.Services;
using System.Data.Common;

namespace MinimumApi.Repositories
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAllPeopleAsync();
        Task<Person> GetPersonByIdAsync(int id);
        Task<Person> GetPersonByNameAsync(string name);
        Task AddPersonAsync(Person person);
        Task UpdatePersonAsync(Person person);
        Task DeletePersonAsync(int id);
    }
}
