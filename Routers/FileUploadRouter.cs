using MinimimApi.Entities;

namespace MinimimApi.Routers 
{
    public class FileUploadRouter : RouterBase 
    {
        public FileUploadRouter(ILogger<CustomerRouter> logger)
        {
            ResourceName = "fileUploads";
            Logger = logger;
        }

     public override void AddRoutes(WebApplication app)
        {
            app.MapPost($"/{ResourceName}", async (IFormFile file) =>
            {
                if (file is null)
                {
                    return Results.BadRequest();
                }

                var uploads = Path.Combine(app.Environment.ContentRootPath, file.FileName);
                await using var fileStream = File.OpenWrite(uploads);
                await using var uploadStream = file.OpenReadStream();
                await uploadStream.CopyToAsync(fileStream);

                return Results.NoContent();
            })
            .Accepts<IFormFile>("multipart/form-data");
        }
    }
}