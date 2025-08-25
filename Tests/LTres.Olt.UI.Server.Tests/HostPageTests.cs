using Microsoft.AspNetCore.Mvc.Testing;

namespace LTres.Olt.UI.Server.Tests;

public class HostPageTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task HostPage_ShouldReturnACorrectLayout()
    {
        var response = await _client.GetAsync("/");

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();
        Assert.Contains("<div id=\"app\">", content);
        Assert.Contains("<svg class=\"loading-progress\">", content);
        Assert.Contains("<script asp-add-nonce src=\"_framework/blazor.webassembly.js\">", content);
    }
}
