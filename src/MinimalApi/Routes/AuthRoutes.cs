
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json.Nodes;

namespace MinimimApi.Routers 
{
    public static class AuthRoutes
    {
        public static void UseAuthRoutes(this WebApplication app)
        {
            app.MapGet("/auth", () =>
            {
                var authCommand = " user-jwts create --scope \"greetings_api\" --role \"admin\" ";
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = "dotnet.exe";
                proc.StartInfo.Arguments = authCommand;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();

                var output = proc.StandardOutput.ReadToEnd();
                var tokenLabel = "Token: ";
                var accessToken = output.Substring(output.IndexOf(tokenLabel) + tokenLabel.Length).Replace("\r\n", "");

                //return TypedResults.Ok(new { accessToken});
                return TypedResults.Content(accessToken);
            }).WithTags("auth");
        }
    }
}