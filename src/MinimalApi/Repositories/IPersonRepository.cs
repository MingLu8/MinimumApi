using MinimumApi.Entities;
using MinimumApi.Extensions;
using MinimumApi.Services;
using RepoDb;
using System.Data;
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

    public class PersonRepository(IDbConnection connection) : IPersonRepository
    {
        public Task<IEnumerable<Person>> GetAllPeopleAsync() => connection.QueryAllAsync<Person>();
        public Task<Person> GetPersonByIdAsync(int id) => connection.QuerySingleAsync<Person>(e => e.Id == id);
        public Task<Person> GetPersonByNameAsync(string name) => connection.QuerySingleAsync<Person>(e => e.Name == name);
        public Task AddPersonAsync(Person person) => connection.InsertAsync<Person>(person);
        public Task UpdatePersonAsync(Person person) => connection.UpdateAsync<Person>(person);
        public Task DeletePersonAsync(int id) => connection.DeleteAsync<Person>(id);
    }
}
