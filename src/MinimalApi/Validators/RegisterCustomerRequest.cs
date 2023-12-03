using FluentValidation;

namespace MinimumApi.Validators
{
    public class RegisterCustomerRequest
    {
        public int? Age { get; set; }
        public string? Name { get; set; }

        public class Validator : AbstractValidator<RegisterCustomerRequest>
        {
            public Validator()
            {
                RuleFor(x => x.Age).NotEmpty().GreaterThan(0);
                RuleFor(x => x.Name).NotEmpty();
            }
        }
    }
}
