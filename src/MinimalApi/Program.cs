using Azure;
using Azure.Core;
using HotChocolate;
using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Models;
using MinimimApi.Routers;
using RepoDb;
using System.Data;

var builder = WebApplication.CreateBuilder(args);
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


builder.Services.AddCors();
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorizationBuilder()
  .AddPolicy("admin_greetings", policy =>
        policy
            .RequireRole("admin")
            .RequireClaim("scope", "greetings_api"));;


var connectionString = builder.Configuration.GetConnectionString("minimalApi");
builder.Services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));
builder.Services.AddScoped<RouterBase, HealthCheckRouter>();
builder.Services.AddScoped<RouterBase, AuthRouter>();
builder.Services.AddScoped<RouterBase, CustomerRouter>();
builder.Services.AddScoped<RouterBase, PersonRouter>();
builder.Services.AddScoped<RouterBase, FileUploadRouter>();
builder.Services.AddScoped<RouterBase, FileDownloadRouter>();

var app = builder.Build();
app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseHttpsRedirection();

//Use Cors need NuGet Package for it.
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
.WithOrigins("https://localhost:5296", "http://localhost:64714"));

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    // Instance of services where you build all RouterBase classes
    var services = scope.ServiceProvider.GetServices<RouterBase>();
    
    // Loop through each RouterBase class
    foreach (var item in services)
    {
        // Invoke the AddRoutes() method for each RouterBase class
        item.AddRoutes(app);
    }
}

app.MapGet("/users/{id:int}", (int id) 
    => id <= 0 ? Results.BadRequest() : Results.Ok(new {id}) );

app.MapGet("/exception", () 
    => { throw new InvalidOperationException("Sample Exception"); });

GlobalConfiguration
    .Setup()
    .UseSqlServer();

app.Run();

public partial class Program { }