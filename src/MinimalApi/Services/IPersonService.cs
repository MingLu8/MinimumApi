using MinimumApi.Entities;
using System.Data.Common;

namespace MinimumApi.Services
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetAllPeopleAsync();
        Task<Person> GetPersonByIdAsync(int id);
        Task<Person> GetPersonByNameAsync(string name);
        Task AddPersonAsync(Person person);
        Task UpdatePersonAsync(Person person);
        Task DeletePersonAsync(int id);
    }
}
