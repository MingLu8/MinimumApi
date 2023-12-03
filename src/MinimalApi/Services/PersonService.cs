using FluentValidation;
using MinimumApi.Entities;
using MinimumApi.Exceptions;
using MinimumApi.Repositories;

namespace MinimumApi.Services
{
    public class PersonService(IValidator<Person> validator, IPersonRepository personRepository) : IPersonService
    {
        public Task<IEnumerable<Person>> GetAllPeopleAsync() => personRepository.GetAllPeopleAsync();
        public Task<Person> GetPersonByIdAsync(long id) => personRepository.GetPersonByIdAsync(id);
        public Task<Person> GetPersonByNameAsync(string name) => personRepository.GetPersonByNameAsync(name);
        public Task AddPersonAsync(Person person)
        {
            if(person.CreatedDateUtc == null)
                person.CreatedDateUtc = DateTime.UtcNow;

            return personRepository.AddPersonAsync(person);
        }

        public async Task UpdatePersonAsync(Person person)
        {
            var validationResult = await validator.ValidateAsync(person);
            if (!validationResult.IsValid)
                throw new ValidationFailedException("Update person failed.", validationResult.ToDictionary());
                
            await personRepository.UpdatePersonAsync(person);
        }

        public Task DeletePersonAsync(long id) => personRepository.DeletePersonAsync(id);
      
    }
}
