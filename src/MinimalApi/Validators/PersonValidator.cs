using FluentValidation;
using MinimumApi.Entities;
using MinimumApi.Repositories;
using Swashbuckle.AspNetCore.Swagger;

namespace MinimumApi.Validators
{
    public class PersonValidator : AbstractValidator<Person>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PersonValidator(IPersonRepository personRepository, IHttpContextAccessor httpContextAccessor)
        {
            _personRepository = personRepository;
            _httpContextAccessor = httpContextAccessor;

            RuleFor(m => m.Name).NotEmpty();
            RuleFor(m => m.Age).NotEmpty().GreaterThan(0);
            RuleFor(person => person.Id)
                .MustAsync(ExistAsync)
                .When(IsPostRequest)
                .WithMessage(p => $"The person with id: {p.Id} does not exist.");
        }

        private bool IsPostRequest(Person person)
        {
            return _httpContextAccessor.HttpContext?.Request.Method == "PUT";
        }

        private async Task<bool> ExistAsync(long id, CancellationToken token)
        {
            return await _personRepository.ExistsAsync(id);              
        }      
    }
}
