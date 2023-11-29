
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json.Nodes;

namespace MinimimApi.Routers 
{
    public class HealthCheckRouter : RouterBase 
    {
    
        public HealthCheckRouter(ILogger<HealthCheckRouter> logger): base("health", logger)
        {

        }

        public override void AddRoutes(WebApplication app)
        {
            var authRoutes = app.MapGroup(ResourceName).WithTags(ResourceName);
            authRoutes.MapGet("/ping", () =>
            {

                return TypedResults.Ok(DateTime.Now);
            }).AddEndpointFilter(async (efiContext, next) =>
            {
                var startTime = DateTime.Now;
                var result = await next(efiContext);
                var typedResult = result as Ok<DateTime>;
                return new {startTime, endTime = typedResult?.Value};
            });
        }
    }
}