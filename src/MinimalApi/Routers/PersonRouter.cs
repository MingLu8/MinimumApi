using HotChocolate.Types.Pagination;
using MinimimApi.Entities;
using MinimumApi.Entities;
using RepoDb;
using System;
using System.Data;
using System.Data.Common;

namespace MinimimApi.Routers 
{
    public class PersonRouter : RouterBase
    {
        public PersonRouter(ILogger<PersonRouter> logger) : base("person", logger)
        {
        }

        public override void AddRoutes(WebApplication app)
        {
            var customerRoutes = app.MapGroup(ResourceName).WithTags(ResourceName);
            customerRoutes.MapGet("/", (IDbConnection dbConnection) => Get(dbConnection));
            customerRoutes.MapGet("/{id:int}", (IDbConnection dbConnection, int id) => Get(dbConnection, id));            
            customerRoutes.MapGet("/{name}", (IDbConnection dbConnection, string name) => GetByName(dbConnection, name));
            customerRoutes.MapPost("/", (IDbConnection dbConnection, Person entity) => Post(dbConnection, entity));
            customerRoutes.MapPut("/{id:int}", (IDbConnection dbConnection, int id, Person entity) => Put(dbConnection, id, entity));
            customerRoutes.MapDelete("/{id:int}", (IDbConnection dbConnection, int id) => Delete(dbConnection, id));
        }

        protected virtual IResult Get(IDbConnection dbConnection)
        {
            Logger?.LogInformation("Getting all people");
            var people = dbConnection.QueryAll<Person>();
            return Results.Ok(people);
        }


        protected virtual IResult Get(IDbConnection dbConnection, int id)
        {
            var person = dbConnection.Query<Person>(e => e.Id == id);
            return person == null ? Results.NotFound() : Results.Ok(person);
        }

        protected virtual IResult GetByName(IDbConnection dbConnection, string name)
        {
            var person = dbConnection.Query<Person>(e => e.Name == name);
            return person == null ? Results.NotFound() : Results.Ok(person);
        }
       
        protected virtual IResult Post(IDbConnection dbConnection, Person person)
        {          
            if(person.CreatedDateUtc == null)
                person.CreatedDateUtc = DateTime.UtcNow;

            var id = dbConnection.Insert(person);
            return Results.Created($"/{ResourceName}/{person.Id}", person);      
        }

        protected virtual IResult Put(IDbConnection dbConnection, int id, Person person)
        {
            if (!dbConnection.Exists<Person>(a => a.Id == id)) 
                return Results.NotFound();
            
            var rowsAffected = dbConnection.Update(person);
            if (rowsAffected == 0) 
                Logger.LogWarning($"No person updated for id: {id}.");

            return Results.Ok(person);
        }

        protected virtual IResult Delete(IDbConnection dbConnection, int id)
        {
            if (!dbConnection.Exists<Person>(a => a.Id == id))
                return Results.NotFound();

            var rowsAffected = dbConnection.Delete<Person>(id);
            if (rowsAffected == 0)
                Logger.LogWarning($"No person updated for id: {id}.");

            return Results.NoContent();
        }
    }

}
