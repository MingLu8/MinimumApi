using Azure;
using Azure.Core;
using Confluent.Kafka;
using FluentValidation;
using HotChocolate;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MinimimApi.Routers;
using MinimumApi.Entities;
using MinimumApi.Extensions;
using MinimumApi.Kafka.Core;
using MinimumApi.Middlewares;
using MinimumApi.Repositories;
using MinimumApi.Routes;
using MinimumApi.Services;
using MinimumApi.Validators;
using RepoDb;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddProblemDetails();
builder.Services.AddHttpClient();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "API",
            Description = "QPIN API with ASP.NET Core 3.0",
            Contact = new OpenApiContact()
            {
                Name = "Tafsir Dadeh Zarrin",
                Url = new Uri("http://www.tdz.co.ir")
            }
        });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            In = ParameterLocation.Header,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            Description = "JWT Authorization header using the Bearer scheme."
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });

    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddCors();
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorizationBuilder()
  .AddPolicy("admin_greetings", policy =>
        policy
            .RequireRole("admin")
            .RequireClaim("scope", "greetings_api"));;

builder.Services.AddAntiforgery();

builder.ConfigKafka();

var database = builder.Configuration.GetValue<string>("database");
if(database == null || database == "sqlite")
{
  var sqliteConnectionString = builder.Configuration.GetConnectionString("minimalApiSqlLite");
  builder.Services.AddScoped<IDbConnection>(_ => new SQLiteConnection(sqliteConnectionString));
  builder.Services.AddScoped<IPersonRepository, PersonSqliteRepository>();
}
else if(database == "sqlServer")
{
  var sqlServerConnectionString = builder.Configuration.GetConnectionString("minimalApiMsSQL");
  builder.Services.AddScoped<IDbConnection>(_ => new SqlConnection(sqlServerConnectionString));
  builder.Services.AddScoped<IPersonRepository, PersonSqlServerRepository>();
}


builder.Services.AddScoped<IPersonService, PersonService>();




builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseStatusCodePages();
app.UseHttpsRedirection();

//Use Cors need NuGet Package for it.
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
.WithOrigins("https://localhost:5296", "http://localhost:64714"));

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseWebAssemblyDebugging();
  app.UseSwagger();
  app.UseSwaggerUI();
  app.UseDeveloperExceptionPage();
}
app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapFallbackToFile("index.html");
app.UsePersonRoutes();
app.UseKafkaRoutes();
app.UseRoutePointsRoutes();
app.UseHealthCheckRoutes();
app.UseAuthRoutes();
app.UseUploadRoutes();
app.UseDatabaseRoutes();

if(database ==  null || database == "sqlite")
    GlobalConfiguration.Setup().UseSQLite();
else if(database == "sqlServer")
  GlobalConfiguration.Setup().UseSqlServer();

app.Run();

public partial class Program { }