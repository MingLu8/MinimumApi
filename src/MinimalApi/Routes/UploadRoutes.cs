namespace MinimimApi.Routers 
{
    public static class UploadRoutes
    {
        public static void AddUploadRoutes(this WebApplication app)
        {
            var uploadDir = $"{app.Environment.ContentRootPath}/uploads";
            if(!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            app.MapPost("uploads", async (IFormFile file) =>
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
            .DisableAntiforgery()
            .RequireAuthorization()
            .Accepts<IFormFile>("multipart/form-data");
        }
    }
}