﻿using GreenDonut;
using MinimumApi.Entities;
using MinimumApi.Services;
using MinimumApi.Validators;
using System;
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

        private async static Task<IResult> AddPersonAsync(Validated<Person> req, IPersonService service, LinkGenerator linker)
        {
            var (isValid, value) = req;

            if (isValid)
            {
                await service.AddPersonAsync(value);
                var location = linker.GetPathByName("getPersonById", new { id = value.Id });
                return TypedResults.Created($"{location}", value);
            }

            return TypedResults.ValidationProblem(req.Errors);
        }

        private async static Task<IResult> UpdatePersonAsync(Validated<Person> req, IPersonService service)
        {
            var (isValid, value) = req;

            if (isValid)
                await service.UpdatePersonAsync(value);

            return TypedResults.NoContent();
        }     
        private async static Task<IResult> DeletePersonAsync(int id, IPersonService service)
        {
            await service.DeletePersonAsync(id);
            return TypedResults.Ok();
        }      
    }
}