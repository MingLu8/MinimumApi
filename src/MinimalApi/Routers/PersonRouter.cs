using HotChocolate.Types.Pagination;
using MinimimApi.Entities;
using MinimumApi.Entities;
using RepoDb;
using System;
using System.Data;

namespace MinimimApi.Routers 
{
    public class PersonRouter : RouterBase
    {
        private readonly IDbConnection _connection;

        public PersonRouter(IDbConnection dbConnection, ILogger<PersonRouter> logger) : base("person", logger)
        {
            _connection = dbConnection;
        }

        public override void AddRoutes(WebApplication app)
        {
            var customerRoutes = app.MapGroup(ResourceName).WithTags(ResourceName);
            customerRoutes.MapGet("/", () => Get()).RequireAuthorization("admin_greetings");
            customerRoutes.MapGet("/{id:int}", (int id) => Get(id));            
            customerRoutes.MapGet("/{name}", (string name) => GetByName(name));
            customerRoutes.MapPost("/", (IDbConnection connection, Person entity) => Post(connection, entity));
            customerRoutes.MapPut("/{id:int}", (int id, Person entity) => Put(id, entity));
            customerRoutes.MapDelete("/{id:int}", (int id) => Delete(id));
        }

        protected virtual IResult Get()
        {
            Logger?.LogInformation("Getting all people");
            var people = _connection.QueryAll<Person>();
            return Results.Ok(people);
        }


        protected virtual IResult Get(int id)
        {
            var person = _connection.Query<Person>(e => e.Id == id);
            return person == null ? Results.NotFound() : Results.Ok(person);
        }

        protected virtual IResult GetByName(string name)
        {
            var person = _connection.Query<Person>(e => e.Name == name);
            return person == null ? Results.NotFound() : Results.Ok(person);
        }
       
        protected virtual IResult Post(IDbConnection connection, Person person)
        {          
            if(person.CreatedDateUtc == null)
                person.CreatedDateUtc = DateTime.UtcNow;
            try
            {
                var id = connection.Insert(person);
                return Results.Created($"/{ResourceName}/{person.Id}", person);
            }
            catch (Exception ex)
            {
                connection.Close();
                return Results.Problem(ex.Message);
            }
        }

        protected virtual IResult Put(int id, Person person)
        {
            if (!_connection.Exists<Person>(a => a.Id == id)) 
                return Results.NotFound();
            
            var rowsAffected = _connection.Update(person);
            if (rowsAffected == 0) 
                Logger.LogWarning($"No person updated for id: {id}.");

            return Results.Ok(person);
        }

        protected virtual IResult Delete(int id)
        {
            if (!_connection.Exists<Person>(a => a.Id == id))
                return Results.NotFound();

            var rowsAffected = _connection.Delete<Person>(id);
            if (rowsAffected == 0)
                Logger.LogWarning($"No person updated for id: {id}.");

            return Results.NoContent();
        }
    }

}
