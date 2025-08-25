using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.Json;
using LTres.Olt.UI.Client.Services;
using LTres.Olt.UI.Shared.Authorization;
using LTres.Olt.UI.Shared.Defaults;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace LTres.Olt.UI.Client.Tests.Services;

public class HostAuthenticationStateProviderTests
{

    private async Task<AuthenticationState> SetUpTestToGetAuthenticationStateAsync(UserInfo? userInfo)
    {
        var fakeJsonResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(userInfo))
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(fakeJsonResponse);
        
        var loggerMock = new Mock<ILogger<HostAuthenticationStateProvider>>();
        var authStateProvider = new HostAuthenticationStateProvider(
            new TestNavigationManager(),
            new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://api.com/") },
            loggerMock.Object);

        return await authStateProvider.GetAuthenticationStateAsync();
    }

    [Fact]
    public async Task GetAuthenticationStateAsync_ShouldReturnAValidState_Normal()
    {
        var userInfo = new UserInfo()
        {
            IsAuthenticated = true,
            Claims = [new(ClaimTypes.Name, "Test User")]
        };

        var resultState = await SetUpTestToGetAuthenticationStateAsync(userInfo);

        Assert.Equal("Test User", resultState.User.Identity?.Name);
    }

    [Fact]
    public async Task GetAuthenticationStateAsync_ShouldReturnAValidState_CustomizedName()
    {
        const string fakeNameClaim = "http://google/fakename";

        var userInfo = new UserInfo()
        {
            IsAuthenticated = true,
            NameClaimType = fakeNameClaim,
            Claims = [new(fakeNameClaim, "Test User")]
        };

        var resultState = await SetUpTestToGetAuthenticationStateAsync(userInfo);

        Assert.Equal("Test User", resultState.User.Identity?.Name);
    }

    [Fact]
    public async Task GetAuthenticationStateAsync_ShouldReturnAnInvalidState()
    {
        var resultState = await SetUpTestToGetAuthenticationStateAsync(null);
        
        Assert.False(resultState.User.Identity?.IsAuthenticated);
    }

    [Fact]
    public void SignIn_ShouldRedirectToLoginUrl()
    {
        var navigationMock = new TestNavigationManager();
        var loggerMock = new Mock<ILogger<HostAuthenticationStateProvider>>();
        var authStateProvider = new HostAuthenticationStateProvider(
            navigationMock,
            new HttpClient() { BaseAddress = new Uri("http://api.com/") },
            loggerMock.Object);
        
        authStateProvider.SignIn();

        Assert.NotNull(navigationMock.NavigatedToUri);
        Assert.StartsWith($"{navigationMock.BaseUri}{AuthDefaults.LogInPath}?returnUrl=", navigationMock.NavigatedToUri);
        Assert.True(navigationMock.NavigatedForcedLoad);
    }

    [Fact]
    public async Task GetSchemesList_ShouldReturnAListOfSchemes()
    {
        List<string> schemes = ["Google", "Microsoft", "Facebook"];

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(schemes))
            });

        var loggerMock = new Mock<ILogger<HostAuthenticationStateProvider>>();
        var authStateProvider = new HostAuthenticationStateProvider(
            new TestNavigationManager(),
            new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://api.com/") },
            loggerMock.Object);

        var returnedList = await authStateProvider.GetSchemesList();

        Assert.Equal(schemes, returnedList);
    }


    private class TestNavigationManager : NavigationManager
    {
        public TestNavigationManager()
        {
            Initialize("http://app.client.com/", "http://app.client.com/");
        }

        public string? NavigatedToUri { get; private set; }
        public bool NavigatedForcedLoad { get; private set; }

        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            NavigatedToUri = uri;
            NavigatedForcedLoad = forceLoad;
            Uri = ToAbsoluteUri(uri).ToString();
            NotifyLocationChanged(false);
        }
    }
}