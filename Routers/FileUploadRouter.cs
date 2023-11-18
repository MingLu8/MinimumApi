namespace MinimimApi.Routers 
{
    public class FileUploadRouter : RouterBase 
    {
    
        public FileUploadRouter(ILogger<FileUploadRouter> logger): base("uploads", logger)
        {
        }

        public override void AddRoutes(WebApplication app)
        {
            var uploadDir = $"{app.Environment.ContentRootPath}/uploads";
            if(!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            app.MapPost($"/{ResourceName}", async (IFormFile file) =>
            {
                if (file is null)
                {
                    return Results.BadRequest();
                }

                var uploads = Path.Combine($"{app.Environment.ContentRootPath}/uploads", file.FileName);
                await using var fileStream = File.OpenWrite(uploads);
                await using var uploadStream = file.OpenReadStream();
                await uploadStream.CopyToAsync(fileStream);

                return Results.NoContent();
            })
            .Accepts<IFormFile>("multipart/form-data");
        }
    }
}