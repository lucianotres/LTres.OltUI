using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Interop;
using MudBlazor.Services;

namespace LTres.Olt.UI.Client.Tests;

public class AppTests : TestContext
{
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
        this.SetAuthenticationState(false);

        var component = RenderComponent<App>();

        var router = component.FindComponent<Router>();

        Assert.NotNull(router);
        Assert.Contains("Welcome to your new app.", component.Markup);
    }

    [Fact]
    public void App_ShouldRedirectToLoginIfUnauthenticated()
    {
        SetUpServices();
        this.SetAuthenticationState(false);

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
        this.SetAuthenticationState(true);

        var navigationMan = Services.GetRequiredService<FakeNavigationManager>();
        navigationMan.NavigateTo("/Authorized");

        var component = RenderComponent<App>();

        var router = component.FindComponent<Router>();

        Assert.NotNull(router);
        Assert.Contains("You are authorized!</h1>", component.Markup);
    }
}
