
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json.Nodes;

namespace MinimimApi.Routers 
{
    public static class HealthCheckRoutes
    {
        public static void AddHealthCheckRoutes(this WebApplication app)
        {
            app.MapGet("/ping", () =>
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