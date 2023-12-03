using MinimumApi.Entities;
using System.Data.Common;

namespace MinimumApi.Services
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetAllPeopleAsync();
        Task<Person?> GetPersonByIdAsync(long id);
        Task AddPersonAsync(Person person);
        Task UpdatePersonAsync(Person person);
        Task PatchPersonAsync(Person person);
        Task DeletePersonAsync(long id);
    }
}
