using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace MinimalApi.IntegrationTests
{

    public class HealthCheckTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _httpClient;

        public HealthCheckTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient();        
        }

        [Fact]
        public async Task PingTest()
        {
            var response = await _httpClient.GetAsync("/health/ping");
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}