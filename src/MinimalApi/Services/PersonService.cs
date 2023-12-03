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
        public async Task AddPersonAsync(Person person)
        {
            person.CreatedDateUtc ??= DateTime.UtcNow;
            await EnsureValidAsync(person);
            await personRepository.AddPersonAsync(person);
        }

        public async Task UpdatePersonAsync(Person person)
        {
            await EnsureValidAsync(person);
            await personRepository.UpdatePersonAsync(person);
        }
    
        public async Task PatchPersonAsync(Person person)
        {
            var existingRecord = await personRepository.GetPersonByIdAsync(person.Id);
            if (existingRecord == null) throw new ResourceNotFoundException($"person with id: {person.Id} does not exists.");

            existingRecord.Age ??= person.Age;
            existingRecord.Name ??= person.Name;
            existingRecord.CreatedDateUtc ??= person.CreatedDateUtc;

            await EnsureValidAsync(existingRecord);
            await personRepository.UpdatePersonAsync(existingRecord);
        }


        public Task DeletePersonAsync(long id) => personRepository.DeletePersonAsync(id);

        private async Task EnsureValidAsync(Person person)
        {
            var validationResult = await validator.ValidateAsync(person);
            if (!validationResult.IsValid)
                throw new ValidationFailedException("Invalid person data.", validationResult.ToDictionary());
        }
    }
}
