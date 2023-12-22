using Confluent.Kafka;
using FluentValidation;
using GreenDonut;
using Microsoft.Extensions.Configuration;
using MinimumApi.Entities;
using MinimumApi.Services;
using MinimumApi.Validators;
using System;
using System.Data;
using System.Net;

namespace MinimumApi.Routes
{
    public static class PersonRoutes
    {
        public static void AddPersonRoutes(this WebApplication app)
        {           
            var customerRoutes = app.MapGroup("people").WithTags("person");
            customerRoutes.MapGet("/", GetAllPeopleAsync);
            customerRoutes.MapGet("/{id:long}", GetPersonByIdAsync).WithName("getPersonById");
            //customerRoutes.MapGet("/{name}", GetPersonByNameAsync);
            customerRoutes.MapPost("/", AddPersonAsync);
            customerRoutes.MapPut("/{id:long}", UpdatePersonAsync);
            customerRoutes.MapPatch("/{id:long}", PatchPersonAsync);
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
        //private async static Task<IResult> SearchPersonAsync(string name, IPersonService service)
        //{
        //    var result = await service.GetPersonByNameAsync(name);
        //    return TypedResults.Ok(result);
        //}

        private async static Task<IResult> AddPersonAsync(Person person, IPersonService service, LinkGenerator linker)
        {
            await service.AddPersonAsync(person);
            var location = linker.GetPathByName("getPersonById", new { id = person.Id });
            return TypedResults.Created($"{location}", person);
        }

        private async static Task<IResult> UpdatePersonAsync(long id, Person person, IPersonService service)
        {
            person.Id = id;
            await service.UpdatePersonAsync(person);
            return TypedResults.NoContent();
        }

        private async static Task<IResult> PatchPersonAsync(long id, Person person, IPersonService service)
        {
            person.Id = id;
            await service.PatchPersonAsync(person);
            return TypedResults.NoContent();
        }

        private async static Task<IResult> DeletePersonAsync(long id, IPersonService service)
        {
            await service.DeletePersonAsync(id);
            return TypedResults.Ok();
        }      
    }
}
