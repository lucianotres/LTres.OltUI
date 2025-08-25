using Bunit;
using LTres.Olt.UI.Client.Layout;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace LTres.Olt.UI.Client.Tests.Layout;

public class LoginDisplayTests : TestContext
{
    [Fact]
    public void ShouldRenderLoginButton_WhenNotAuthorized()
    {
        this.SetAuthenticationState(false);

        var component = RenderComponent<CascadingAuthenticationState>(p => p.AddChildContent<LoginDisplay>());
        var loginLink = component.FindComponent<MudLink>();

        Assert.NotNull(component);
        Assert.Equal("/Login", loginLink.Instance.Href);
    }
    
    [Fact]
    public void ShouldRenderLogoutButton_WhenAuthorized()
    {
        this.SetAuthenticationState(true);
        
        var component = RenderComponent<CascadingAuthenticationState>(p => p.AddChildContent<LoginDisplay>());
        var loginLink = component.FindComponent<MudLink>();
        
        Assert.NotNull(component);
        Assert.Equal("api/Account/Logout", loginLink.Instance.Href);
    }
    
}