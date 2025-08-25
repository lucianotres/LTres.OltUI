using System.Net;
using System.Security.Claims;
using LTres.Olt.UI.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Moq;

namespace LTres.Olt.UI.Client.Tests.Services;

public class AuthorizedHandlerTests
{
    private HttpClient SetUpClient(bool anAuthenticatedState)
    {
        ClaimsIdentity identity = anAuthenticatedState ?
            new([new Claim(ClaimTypes.Name, "Test User")], "TestAuthentication") :
            new();

        ClaimsPrincipal principal = new(identity);
        AuthenticationState authState = new(principal);

        var loggerMock = new Mock<ILogger<HostAuthenticationStateProvider>>();
        var authenticationStateProviderMock = new Mock<HostAuthenticationStateProvider>(
            new TestNavigationManager(), new HttpClient(), loggerMock.Object
        );
        authenticationStateProviderMock.Setup(x => x.GetAuthenticationStateAsync())
            .ReturnsAsync(authState);
        
        var authorizedHandler = new AuthorizedHandler(authenticationStateProviderMock.Object)
        {
            InnerHandler = new TestHandler()
        };

        return new HttpClient(authorizedHandler);
    }

    [Fact]
    public async Task SendAsync_ShouldHandleUnauthorizedUser()
    {
        var client = SetUpClient(false);

        var response = await client.GetAsync("http://localhost/test");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task SendAsync_ShouldHandleAuthorizedUser()
    {
        var client = SetUpClient(true);

        var response = await client.GetAsync("http://localhost/test");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Test result!", await response.Content.ReadAsStringAsync());
    }



    private class TestHandler : HttpMessageHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Test result!")
            });
        }
    }

    private class TestNavigationManager : NavigationManager
    {
        public TestNavigationManager()
        {
            Initialize("http://localhost/", "http://localhost/");
        }

        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            Uri = ToAbsoluteUri(uri).ToString();
            NotifyLocationChanged(false);
        }
    }
}