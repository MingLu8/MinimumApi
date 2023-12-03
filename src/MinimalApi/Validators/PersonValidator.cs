using FluentValidation;
using MinimumApi.Entities;

namespace MinimumApi.Validators
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
            RuleFor(m => m.Age).NotEmpty().GreaterThan(0);
        }
    }
}
