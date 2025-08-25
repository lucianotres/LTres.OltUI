
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

public class TestAuthorizationService(bool autorize) : IAuthorizationService
{
    private readonly bool _authorize = autorize;

    public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, IEnumerable<IAuthorizationRequirement> requirements)
        => Task.FromResult(_authorize ? AuthorizationResult.Success() : AuthorizationResult.Failed());

    public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, string policyName)
        => Task.FromResult(_authorize ? AuthorizationResult.Success() : AuthorizationResult.Failed());
}
