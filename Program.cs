using MinimimApi.Routers;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddProblemDetails();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddScoped<RouterBase, CustomerRouter>();
builder.Services.AddScoped<RouterBase, FileUploadRouter>();
builder.Services.AddScoped<RouterBase, FileDownloadRouter>();

var app = builder.Build();
app.UseExceptionHandler();
app.UseStatusCodePages();
//Use Cors need NuGet Package for it.
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
.WithOrigins("https://localhost:5296", "http://localhost:64714"));


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

app.Map("/users/{id:int}", (int id) 
    => id <= 0 ? Results.BadRequest() : Results.Ok(new {id}) );

app.Map("/exception", () 
    => { throw new InvalidOperationException("Sample Exception"); });


app.Run();
