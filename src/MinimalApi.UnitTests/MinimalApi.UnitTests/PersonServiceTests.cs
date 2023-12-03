using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MinimumApi.Entities;
using MinimumApi.Exceptions;
using MinimumApi.Repositories;
using MinimumApi.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace MinimalApi.UnitTests
{
    public class PersonServiceTests
    {
        [Fact]
        public async Task AddPersonAsync_auto_populates_CreateDateUTC()
        {
            var validator = Substitute.For<IValidator<Person>>();
            var repo = Substitute.For<IPersonRepository>();
            var service = new PersonService(validator, repo);
            var person = new Person { Age = 1, Name = "x" };
            await service.AddPersonAsync(person);
            await repo.Received(1).InsertAsync(Arg.Is<Person>(p=> p.CreatedDateUtc.HasValue));
        }

        [Fact]
        public void UpdatePersonAsync_throws_validation_exception_when_person_does_not_exist()
        {
            var validationResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Age", "error") });
            var validator = Substitute.For<IValidator<Person>>();
            validator.ValidateAsync(Arg.Any<Person>()).Returns(validationResult);

            var repo = Substitute.For<IPersonRepository>();
            var service = new PersonService(validator, repo);
            service.UpdatePersonAsync(new Person())
                .Should().Throws<ValidationFailedException>();
        }
    }
}