using LTres.Olt.UI.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace LTres.Olt.UI.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetCurrentUser() => Ok(CreateUserInfo(User));

    private UserInfo CreateUserInfo(ClaimsPrincipal claimsPrincipal)
    {
        if (!claimsPrincipal?.Identity?.IsAuthenticated ?? true)
        {
            return UserInfo.Anonymous;
        }

        var userInfo = new UserInfo
        {
            IsAuthenticated = true
        };

        if (claimsPrincipal?.Identity is ClaimsIdentity claimsIdentity)
        {
            userInfo.NameClaimType = claimsIdentity.NameClaimType;
            userInfo.RoleClaimType = claimsIdentity.RoleClaimType;
        }
        else
        {
            userInfo.NameClaimType = ClaimTypes.Name;
            userInfo.RoleClaimType = ClaimTypes.Role;
        }

        if (claimsPrincipal?.Claims?.Any() ?? false)
        {
            //send all claims to client
            var claims = claimsPrincipal.Claims
                .Select(u => new ClaimValue(u.Type, u.Value))
                .ToList();

            userInfo.Claims = claims;
        }

        return userInfo;
    }
}
