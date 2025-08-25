
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace LTres.Olt.UI.Client.Tests;

public class TestAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly AuthenticationState _authState;

    public TestAuthenticationStateProvider(AuthenticationState authState)
    {
        _authState = authState;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
        => Task.FromResult(_authState);
}

public class TestAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        => Task.FromResult(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        => Task.FromResult<AuthorizationPolicy?>(null);

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        => Task.FromResult<AuthorizationPolicy?>(
            new AuthorizationPolicyBuilder().RequireClaim(policyName).Build());
}

public class TestAuthorizationService : IAuthorizationService
{
    public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, IEnumerable<IAuthorizationRequirement> requirements)
        => Task.FromResult(AuthorizationResult.Success());

    public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, string policyName)
        => Task.FromResult(AuthorizationResult.Success());
}
