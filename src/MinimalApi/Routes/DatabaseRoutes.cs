using Microsoft.AspNetCore.Mvc;
using MinimumApi.Extensions;

namespace MinimumApi.Routes
{
  public static class DatabaseRoutes
  {
    public static void UseDatabaseRoutes(this WebApplication app)
    {
      var customerRoutes = app.MapGroup("database").WithTags("database");
      customerRoutes.MapGet("/database", () => app.Configuration.GetValue<string>("database"));
      customerRoutes.MapPost("/create-sqlite-db", CreateSqliteDb);
      customerRoutes.MapPost("/create-sql-server-db/{adminUserId}/{adminUserPassword}", CreateSqlServerDb);
    }

    private static Task CreateSqlServerDb(string adminUserId, string adminUserPassword, IConfiguration config)
    {
      var connectionString = config.GetConnectionString("minimalApiMsSQL");
      return Task.Run(() => DbExtensions.CreateSqlServerDatabaseIfNotExists(connectionString, adminUserId, adminUserPassword, "./Data/baseline.sql"));
    }

    private static Task CreateSqliteDb(IConfiguration config)
    {
      var connectionString = config.GetConnectionString("minimalApiSqlLite");
      return Task.Run(()=> DbExtensions.CreateSQLiteDatabaseIfNotExists(connectionString, "baseline-sqlite.sql"));
    }
  }
}
