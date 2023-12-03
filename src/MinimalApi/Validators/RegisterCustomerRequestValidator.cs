using FluentValidation;

namespace MinimumApi.Validators
{
    public class RegisterCustomerRequestValidator : AbstractValidator<RegisterCustomerRequest>
    {
        public RegisterCustomerRequestValidator()
        {
            RuleFor(x => x.Age).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
