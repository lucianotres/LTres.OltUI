using LTres.Olt.UI.Server.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LTres.Olt.UI.Server.Tests;

public class AccountControllerTests
{
    [Fact]
    public async Task GetSchemes_ShouldReturnAListOfSchemes_WithoutCookies()
    {
        // Arrange
        var testSchemes = new List<AuthenticationScheme>
        {
            new("Google", "Google", typeof(IAuthenticationHandler)),
            new("Microsoft", "Microsoft", typeof(IAuthenticationHandler)),
            new("Cookies", "Cookies", typeof(IAuthenticationHandler))
        };
        var expectedSchemeNames = testSchemes
            .Select(s => s.Name)
            .Where(s => s != "Cookies")
            .ToList();

        var schemeProviderMock = new Mock<IAuthenticationSchemeProvider>();
        schemeProviderMock
            .Setup(p => p.GetAllSchemesAsync())
            .ReturnsAsync(testSchemes);

        var controller = new AccountController();

        var actionResult = await controller.GetSchemes(schemeProviderMock.Object);

        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        var actualSchemes = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
        Assert.Equal(expectedSchemeNames, actualSchemes);
    }

    [Theory]
    [InlineData("", "/")]
    [InlineData("/", "/")]
    [InlineData("/Authorized", "/Authorized")]
    public void Login_ShouldReturnChallenge(string returnUrl, string expectedReturnUrl)
    {
        var controller = new AccountController();

        var actionResult = controller.Login("Google", returnUrl);

        var challengeResult = Assert.IsType<ChallengeResult>(actionResult);
        Assert.NotNull(challengeResult);
        Assert.NotNull(challengeResult.Properties);
        Assert.Equal(expectedReturnUrl, challengeResult.Properties.RedirectUri);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("google")]
    [InlineData("microsoft")]
    public void Logout_ShouldReturnSingOutResult_WithCorrectlyScheme(string? authScheme)
    {
        var claims = new Claim[]
        {
            new("scheme", "Cookies")
        };

        if (authScheme != null)
        {
            claims = [.. claims, new Claim("authentication_scheme", authScheme)];
        }

        var controller = new AccountController
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthentication"))
                }
            }
        };

        string[] expectedAuthSchemes = ["Cookies"];
        if (authScheme != null && authScheme != "google")
        {
            expectedAuthSchemes = [.. expectedAuthSchemes, authScheme];
        }

        var actionResult = controller.Logout();

        var signOutResult = Assert.IsType<SignOutResult>(actionResult);
        Assert.NotNull(signOutResult);
        Assert.NotNull(signOutResult.Properties);
        Assert.Equal(expectedAuthSchemes, signOutResult.AuthenticationSchemes);        
    }
}
