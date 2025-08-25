using Microsoft.AspNetCore.Mvc.Testing;

namespace LTres.Olt.UI.Server.Tests;

public class ErrorPageTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task ErrorPage_ShouldReturnACorrectLayout()
    {
        var response = await _client.GetAsync("/");

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();
        Assert.Contains("<h1 class=\"text-danger\">Error.</h1>", content);
        Assert.Contains("<h2 class=\"text-danger\">An error occurred while processing your request.</h2>", content);
    }
}
