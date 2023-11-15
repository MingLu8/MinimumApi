using MinimimApi.Entities;

namespace MinimimApi.Routers 
{
    public class CustomerRouter : RouterBase 
    {
        public CustomerRouter(ILogger<CustomerRouter> logger)
        {
            ResourceName = "customer";
            Logger = logger;
        }

    public override void AddRoutes(WebApplication app)
    {
        app.MapGet($"/{ResourceName}", () => Get());

        app.MapGet($"/{ResourceName}/{{id:int}}",
        (int id) => Get(id));

        app.MapGet($"/{ResourceName}/{{name}}",
       (string name) => GetByFirstName(name));

        app.MapPost($"/{ResourceName}",
        (Customer entity) => Post(entity));

        app.MapPut($"/{ResourceName}/{{id:int}}",
        (int id, Customer entity) => Put(id, entity));

        app.MapDelete($"/{ResourceName}/{{id:int}}",
       (int id) => Delete(id));
    }

    protected virtual IResult Get()
    {
        Logger.LogInformation("Getting all customers");
        return Results.Ok(GetAll());
    }


    private List<Customer> GetAll()
    {
        return new List<Customer> {
            new Customer {
                CustomerID = 1,
                FirstName = "Orlando",
                LastName = "Gee",
                CompanyName = "A Bike Store",
                EmailAddress = "orlando0@adventure-works.com",
                },
                new Customer {
                CustomerID = 2,
                FirstName = "Keith",
                LastName = "Harris",
                CompanyName = "Progressive Sports",
                EmailAddress = "keith0@adventure-works.com",
                },
                new Customer {
                CustomerID = 3,
                FirstName = "Donna",
                LastName = "Carreras",
                CompanyName = "Advanced Bike Components",
                EmailAddress = "donna0@adventure-works.com",
                },
                new Customer {
                CustomerID = 4,
                FirstName = "Janet",
                LastName = "Gates",
                CompanyName = "Modular Cycle Systems",
                EmailAddress = "janet1@adventure-works.com",
                },
                new Customer {
                CustomerID = 5,
                FirstName = "Lucy",
                LastName = "Harrington",
                CompanyName = "Metropolitan Sports Supply",
                EmailAddress = "lucy0@adventure-works.com",
                }
            };
    }

    /// <summary>
    /// GET a single row of data
    /// </summary>
    /// <returns>An IResult object</returns>
    protected virtual IResult Get(int id)
    {
        // Locate a single row of data
        Customer? current = GetAll()
        .Find(c => c.CustomerID == id);
        if (current != null)
        {
            return Results.Ok(current);
        }
        else
        {
            return Results.NotFound();
        }
    }

    /// <summary>
    /// GET a single row of data
    /// </summary>
    /// <returns>An IResult object</returns>
    protected virtual IResult GetByFirstName(string name)
    {
        // Locate a single row of data
        Customer? current = GetAll()
        .Find(c => c.FirstName == name);
        if (current != null)
        {
            return Results.Ok(current);
        }
        else
        {
            return Results.NotFound();
        }
    }
        /// <summary>
        /// INSERT new data
        /// </summary>
        /// <returns>An IResult object</returns>
        protected virtual IResult Post(Customer entity)
        {
            // Generate a new ID
            entity.CustomerID = GetAll()
            .Max(c => c.CustomerID) + 1;
            // TODO: Insert into data store
            // Return the new object created
            return Results.Created(
            $"/{ResourceName}/{entity.CustomerID}", entity);
        }

        /// UPDATE existing data
        /// </summary>
        /// <returns>An IResult object</returns>
        protected virtual IResult Put(int id, Customer entity)
        {
            IResult ret;
            // Locate a single row of data
            Customer? current = GetAll()
            .Find(c => c.CustomerID == id);
            if (current != null)
            {
                // TODO: Update the entity
                current.FirstName = entity.FirstName;
                current.LastName = entity.LastName;
                current.CompanyName = entity.CompanyName;
                current.EmailAddress = entity.EmailAddress;
                // TODO: Update the data store
                // Return the updated entity
                ret = Results.Ok(current);
            }
            else
            {
                ret = Results.NotFound();
            }

            return ret;
        }

        /// <summary>
        /// DELETE a single row
        /// </summary>
        /// <returns>An IResult object</returns>
        protected virtual IResult Delete(int id)
        {
            IResult ret;
            // Locate a single row of data
            Customer? current = GetAll()
            .Find(c => c.CustomerID == id);
            if (current != null)
            {
                // TODO: Delete data from the data store
                GetAll().Remove(current);
                // Return NoContent
                ret = Results.NoContent();
            }
            else
            {
                ret = Results.NotFound();
            }
            return ret;
        }
    }

}
