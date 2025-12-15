using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace Cinema.Tests;

public class AuthTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthTests(WebApplicationFactory<Program> factory)
    {
        Environment.SetEnvironmentVariable("JWT__SigningKey", "test-secret-key-very-long");
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Reserve_Without_Token_Returns_401()
    {
        var res = await _client.PostAsync("/api/tickets/reserve", null);
        Assert.Equal(HttpStatusCode.Unauthorized, res.StatusCode);
    }
}