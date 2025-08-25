using System.Security.Claims;
using LTres.Olt.UI.Server.Controllers;
using LTres.Olt.UI.Shared.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LTres.Olt.UI.Server.Tests;

public class UserControllerTests
{

    [Fact]
    public void GetCurrentUser_ShouldReturnCurrentUser_Authenticated()
    {
        UserController userController = new()
        {
            ControllerContext = new()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(
                        [new("scheme", "Cookies"), new(ClaimTypes.Name, "Test"), new(ClaimTypes.Role, "Admin")],
                        "TestAuthentication"))
                }
            }
        };

        var currentUserResult = userController.GetCurrentUser();

        var okResult = Assert.IsAssignableFrom<OkObjectResult>(currentUserResult);
        var currentUser = Assert.IsAssignableFrom<UserInfo>(okResult.Value);
        Assert.NotNull(currentUser);
        Assert.True(currentUser.IsAuthenticated);
        Assert.Equal("Test", currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value);
        Assert.Equal("Admin", currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value);
    }

    [Fact]
    public void GetCurrentUser_ShouldReturnCurrentUser_Anonymous()
    {
        UserController userController = new()
        {
            ControllerContext = new()
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        var currentUserResult = userController.GetCurrentUser();

        var okResult = Assert.IsAssignableFrom<OkObjectResult>(currentUserResult);
        var currentUser = Assert.IsAssignableFrom<UserInfo>(okResult.Value);
        Assert.NotNull(currentUser);
        Assert.False(currentUser.IsAuthenticated);
    }
}