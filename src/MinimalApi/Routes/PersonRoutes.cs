using FluentValidation;
using GreenDonut;
using MinimumApi.Entities;
using MinimumApi.Services;
using MinimumApi.Validators;
using System;
using System.Data;

namespace MinimumApi.Routes
{
    public static class PersonRoutes
    {
        public static void AddPersonRoutes(this RouteGroupBuilder root)
        {
            var customerRoutes = root.MapGroup("people").WithTags("person");
            customerRoutes.MapGet("/", GetAllPeopleAsync);
            customerRoutes.MapGet("/{id:long}", GetPersonByIdAsync).WithName("getPersonById");
            customerRoutes.MapGet("/{name}", GetPersonByNameAsync);
            customerRoutes.MapPost("/", AddPersonAsync);
            customerRoutes.MapPut("/{id:long}", UpdatePersonAsync);
            customerRoutes.MapDelete("/{id:long}", DeletePersonAsync);          
        }

        private async static Task<IResult> GetAllPeopleAsync(IPersonService service)
        {
            var result = await service.GetAllPeopleAsync();
            return TypedResults.Ok(result);
        }
                
        private async static Task<IResult> GetPersonByIdAsync(long id, IPersonService service)
        {
            var result = await service.GetPersonByIdAsync(id);
            return TypedResults.Ok(result);
        }
        private async static Task<IResult> GetPersonByNameAsync(string name, IPersonService service)
        {
            var result = await service.GetPersonByNameAsync(name);
            return TypedResults.Ok(result);
        }

        private async static Task<IResult> AddPersonAsync2([Validate] Person person, IPersonService service, LinkGenerator linker)
        {
            await service.AddPersonAsync(person);
            var location = linker.GetPathByName("getPersonById", new { id = person.Id });
            return TypedResults.Created($"{location}", person);          
        }

        private async static Task<IResult> AddPersonAsync(Person person, IPersonService service, LinkGenerator linker, IValidator<Person> validator)
        {
            var validationResult = validator.Validate(person);
            if (validationResult.IsValid)
            {
                await service.AddPersonAsync(person);
                var location = linker.GetPathByName("getPersonById", new { id = person.Id });
                return TypedResults.Created($"{location}", person);
            }
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        private async static Task<IResult> UpdatePersonAsync(long id, Person person, IPersonService service, IValidator<Person> validator)
        {
            person.Id = id;
            var validationResult = await validator.ValidateAsync(person);
            if (validationResult.IsValid)
            {
                await service.UpdatePersonAsync(person);
                return TypedResults.NoContent();
            }
            return Results.ValidationProblem(validationResult.ToDictionary());
        }
     
        private async static Task<IResult> DeletePersonAsync(long id, IPersonService service)
        {
            await service.DeletePersonAsync(id);
            return TypedResults.Ok();
        }      
    }
}
