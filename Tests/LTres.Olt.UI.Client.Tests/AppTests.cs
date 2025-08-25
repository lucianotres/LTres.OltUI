using System.Security.Claims;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Interop;
using MudBlazor.Services;

namespace LTres.Olt.UI.Client.Tests;

public class AppTests : TestContext
{
    private void SetAuthenticationState(bool anAuthenticatedState)
    {
        ClaimsIdentity identity = anAuthenticatedState ?
            new([new Claim(ClaimTypes.Name, "Test User")], "TestAuthentication") :
            new();

        ClaimsPrincipal principal = new(identity);
        AuthenticationState authState = new(principal);

        Services.AddSingleton<AuthenticationStateProvider>(new TestAuthenticationStateProvider(authState));
        Services.AddSingleton<IAuthorizationPolicyProvider>(new TestAuthorizationPolicyProvider());
        Services.AddSingleton<IAuthorizationService>(new TestAuthorizationService(anAuthenticatedState));
    }

    private void SetUpServices()
    {
        JSInterop.SetupVoid("mudKeyInterceptor.connect", _ => true);
        JSInterop.Setup<BoundingClientRect>("mudElementRef.getBoundingClientRect", _ => true);
        JSInterop.Setup<int>("mudpopoverHelper.countProviders");
        JSInterop.SetupVoid("watchDarkThemeMedia", _ => true);

        Services.AddLocalization();
        Services.AddMudServices();
    }

    [Fact]
    public void App_ShouldRenderCorrectlyByDefaultRoute()
    {
        SetUpServices();
        SetAuthenticationState(false);

        var component = RenderComponent<App>();

        var router = component.FindComponent<Router>();

        Assert.NotNull(router);
        Assert.Contains("Welcome to your new app.", component.Markup);
    }

    [Fact]
    public void App_ShouldRedirectToLoginIfUnauthenticated()
    {
        SetUpServices();
        SetAuthenticationState(false);

        var navigationMan = Services.GetRequiredService<FakeNavigationManager>();
        navigationMan.NavigateTo("/Authorized");

        var component = RenderComponent<App>();

        var router = component.FindComponent<Router>();

        Assert.NotNull(router);
        Assert.Contains("<div class=\"enter-with\"", component.Markup);
    }
    
    [Fact]
    public void App_ShouldRenderAuthorizedPageIfAuthenticated()
    {
        SetUpServices();
        SetAuthenticationState(true);

        var navigationMan = Services.GetRequiredService<FakeNavigationManager>();
        navigationMan.NavigateTo("/Authorized");

        var component = RenderComponent<App>();

        var router = component.FindComponent<Router>();

        Assert.NotNull(router);
        Assert.Contains("You are authorized!</h1>", component.Markup);
    }
}
