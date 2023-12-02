using MinimumApi.Entities;
using MinimumApi.Repositories;

namespace MinimumApi.Services
{
    public class PersonService(IPersonRepository personRepository) : IPersonService
    {
        public Task<IEnumerable<Person>> GetAllPeopleAsync() => personRepository.GetAllPeopleAsync();
        public Task<Person> GetPersonByIdAsync(int id) => personRepository.GetPersonByIdAsync(id);
        public Task<Person> GetPersonByNameAsync(string name) => personRepository.GetPersonByNameAsync(name);
        public Task AddPersonAsync(Person person)
        {
            if(person.CreatedDateUtc == null)
                person.CreatedDateUtc = DateTime.UtcNow;

            return personRepository.AddPersonAsync(person);
        }

        public Task UpdatePersonAsync(Person person) => personRepository.UpdatePersonAsync(person);
        public Task DeletePersonAsync(int id) => personRepository.DeletePersonAsync(id);
      
    }
}
