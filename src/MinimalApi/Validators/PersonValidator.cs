using FluentValidation;
using MinimumApi.Entities;
using MinimumApi.Repositories;

namespace MinimumApi.Validators
{
    public class PersonValidator : AbstractValidator<Person>
    {
        private readonly IPersonRepository _personRepository;

        public PersonValidator(IPersonRepository personRepository)
        {
            RuleFor(m => m.Name).NotEmpty();
            RuleFor(m => m.Age).NotEmpty().GreaterThan(0);
            RuleFor(person => person.Id).MustAsync(ExistsOnDB).WithMessage(p => $"The person with id: {p.Id} does not exist");
            _personRepository = personRepository;
        }

        private Task<bool> ExistsOnDB(long id, CancellationToken token)
        {
            return _personRepository.ExistsAsync(id);
        }
    }
}
