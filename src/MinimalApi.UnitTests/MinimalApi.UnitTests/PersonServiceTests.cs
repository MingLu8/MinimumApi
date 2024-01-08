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
      await repo.Received(1).InsertAsync(Arg.Is<Person>(p => p.CreatedDateUtc.HasValue));
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

    [Fact]
    public void FindDupTest()
    {
      var sample = new int[] { 1, 2, 2, 3 };
      var dup = FindDup(sample);
      dup.Should().Be(2);
    }

    [Fact]
    public void FindPrefixTest()
    {
      var sample = new string[] { "ab", "ab", "a" };
      var dup = FindPrefix(sample);
      dup.Should().Be("a");
    }

    private string FindPrefix(string[] input)
    {
      if (input == null || input.Length == 0) return "";

      var minLength = input.Min(x => x.Length);
      var prefix = input[0][0].ToString();
      for(var i = 0; i < minLength; i++)
      {
        if(input.All(a=> a[0..i] == prefix))
          prefix += input[0][i];        
      }
      return prefix;
    }

    private int? FindDup(int[] input)
    {
      for(int i = 0; i < input.Length; i++) 
      {
        for (int j = 0; j < input.Length; j++)
        {
          if(i != j && input[i] == input[j])
            return input[i];
        }
      }
      return null;
    }
  }
}