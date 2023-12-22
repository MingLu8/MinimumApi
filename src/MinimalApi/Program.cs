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
using MinimumApi.Kafka;
using MinimumApi.Middlewares;
using MinimumApi.Repositories;
using MinimumApi.Routes;
using MinimumApi.Services;
using MinimumApi.Validators;
using RepoDb;
using System.Data;
using System.Data.SQLite;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddProblemDetails();

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

var consumerConfig = new ConsumerConfig();
builder.Configuration.Bind("ConsumerConfig", consumerConfig);
builder.Services.AddSingleton(_=>consumerConfig);
builder.Services.AddSingleton<IConsumer, Consumer>();


var producerConfig = new ProducerConfig();
builder.Configuration.Bind("ProducerConfig", producerConfig);
builder.Services.AddSingleton(_ => producerConfig);
builder.Services.AddSingleton<IProducer, Producer>();

//var connectionString = builder.Configuration.GetConnectionString("minimalApiMsSQL");
//builder.Services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));

var connectionString = builder.Configuration.GetConnectionString("minimalApiSqlLite");
builder.Services.AddScoped<IDbConnection>(_ => new SQLiteConnection(connectionString));

builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPersonRepository, PersonRepoDbRepository>();


builder.Services.AddValidatorsFromAssemblyContaining<Program>();

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
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseDeveloperExceptionPage();
}

app.AddPersonRoutes(consumerConfig);
app.AddKafkaRoutes();
app.AddHealthCheckRoutes();
app.AddAuthRoutes();
app.AddUploadRoutes();

GlobalConfiguration
    .Setup()
    //.UseSqlServer()
    .UseSQLite();

app.Run();

public partial class Program { }