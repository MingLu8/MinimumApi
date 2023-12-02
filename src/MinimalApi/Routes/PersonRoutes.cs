using GreenDonut;
using MinimumApi.Entities;
using MinimumApi.Services;
using System.Data;

namespace MinimumApi.Routes
{
    public static class PersonRoutes
    {
        public static void AddPersonRoutes(this WebApplication app)
        {
            var customerRoutes = app.MapGroup("people").WithTags("person");
            customerRoutes.MapGet("/", GetAllPeopleAsync);
            customerRoutes.MapGet("/{id:int}", GetPersonByIdAsync).WithName("getPersonById");
            customerRoutes.MapGet("/{name}", GetPersonByNameAsync);
            customerRoutes.MapPost("/", AddPersonAsync);
            customerRoutes.MapPut("/{id:int}", UpdatePersonAsync);
            customerRoutes.MapDelete("/{id:int}", DeletePersonAsync);          
        }

        private async static Task<IResult> GetAllPeopleAsync(IPersonService service)
        {
            var result = await service.GetAllPeopleAsync();
            return TypedResults.Ok(result);
        }
                
        private async static Task<IResult> GetPersonByIdAsync(int id, IPersonService service)
        {
            var result = await service.GetPersonByIdAsync(id);
            return TypedResults.Ok(result);
        }
        private async static Task<IResult> GetPersonByNameAsync(string name, IPersonService service)
        {
            var result = await service.GetPersonByNameAsync(name);
            return TypedResults.Ok(result);
        }

        private async static Task<IResult> AddPersonAsync(Person person, IPersonService service, LinkGenerator linker)
        {
            await service.AddPersonAsync(person);
            var location = linker.GetPathByName("getPersonById", new { id = person.Id });
            return TypedResults.Created($"{location}", person);
        }

        private async static Task<IResult> UpdatePersonAsync(Person person, IPersonService service)
        {
            await service.UpdatePersonAsync(person);
            return TypedResults.NoContent();
        }     
        private async static Task<IResult> DeletePersonAsync(int id, IPersonService service)
        {
            await service.DeletePersonAsync(id);
            return TypedResults.Ok();
        }      
    }
}
