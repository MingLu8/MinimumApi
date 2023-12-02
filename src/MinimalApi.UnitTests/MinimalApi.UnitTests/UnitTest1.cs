using MinimumApi.Entities;
using MinimumApi.Repositories;
using MinimumApi.Services;
using NSubstitute;

namespace MinimalApi.UnitTests
{
    public class PersonServiceTests
    {
        [Fact]
        public void CreateDateUTC_is_auto_populated()
        {
            var repo = Substitute.For<IPersonRepository>();
            var service = new PersonService(repo);
            var person = new Person { Age = 1, Name = "x" };
            service.AddPersonAsync(person);
            repo.Received(1).AddPersonAsync(Arg.Is<Person>(p=> p.CreatedDateUtc.HasValue));
        }
    }
}