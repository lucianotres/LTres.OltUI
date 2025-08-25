using Bunit;
using Bunit.TestDoubles;
using LTres.Olt.UI.Client.Layout;
using Microsoft.Extensions.DependencyInjection;

namespace LTres.Olt.UI.Client.Tests.Layout;

public class RedirectToLoginTests : TestContext
{
    [Fact]
    public void OnInitialized_ShouldNavigateToLogin()
    {
        var component = RenderComponent<RedirectToLogin>();
        var navigationMan = Services.GetRequiredService<FakeNavigationManager>();

        Assert.NotNull(component);
        Assert.StartsWith("http://localhost/Login?ReturnUrl=", navigationMan.Uri);
    }
}