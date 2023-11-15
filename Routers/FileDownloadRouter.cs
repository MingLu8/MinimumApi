
using System.Text;

namespace MinimimApi.Routers 
{
    public class FileDownloadRouter : RouterBase 
    {
    
        public FileDownloadRouter(ILogger<FileUploadRouter> logger)
        {
            ResourceName = "downloads";
            Logger = logger;

        }

        public override void AddRoutes(WebApplication app)
        {
           app.MapGet($"{ResourceName}", () =>
            {
                var barr =  Encoding.ASCII.GetBytes("some content....");
                return Results.File(barr, "text/txt", "some-content.txt");
            });
        }
    }
}