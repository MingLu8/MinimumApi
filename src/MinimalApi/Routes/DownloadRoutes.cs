
using System.Text;

namespace MinimimApi.Routers 
{
    public static class DownloadRoutes
    {
        public static void AddDownloadRoutes(this WebApplication app)
        {
            app.MapGet("/download", () =>
            {
                var barr =  Encoding.ASCII.GetBytes("some content....");
                return Results.File(barr, "text/txt", "some-content.txt");
            });
        }
    }
}