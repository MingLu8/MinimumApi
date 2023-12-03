using MinimumApi.Entities;
using MinimumApi.Extensions;
using RepoDb;
using System.Data;

namespace MinimumApi.Repositories
{
    public class PersonRepository(IDbConnection connection) : IPersonRepository
    {
        public Task<IEnumerable<Person>> GetAllPeopleAsync() => connection.QueryAllAsync<Person>();
        public Task<Person> GetPersonByIdAsync(long id) => connection.QuerySingleAsync<Person>(e => e.Id == id);
        public Task<bool> ExistsAsync(long id) => connection.ExistsAsync<Person>(e => e.Id == id);      
        public Task<Person> GetPersonByNameAsync(string name) => connection.QuerySingleAsync<Person>(e => e.Name == name);
        public Task AddPersonAsync(Person person) => connection.InsertAsync<Person>(person);
        public Task UpdatePersonAsync(Person person) => connection.UpdateAsync<Person>(person);
        public Task DeletePersonAsync(long id) => connection.DeleteAsync<Person>(id);
    }
}
