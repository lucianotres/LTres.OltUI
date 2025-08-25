
using System.Security.Claims;
using Bunit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

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


public static class TestAuthorizationExtensions
{
    public static void SetAuthenticationState(this TestContext context, bool anAuthenticatedState)
    {
        ClaimsIdentity identity = anAuthenticatedState ?
            new([new Claim(ClaimTypes.Name, "Test User")], "TestAuthentication") :
            new();

        ClaimsPrincipal principal = new(identity);
        AuthenticationState authState = new(principal);

        context.Services.AddSingleton<AuthenticationStateProvider>(new TestAuthenticationStateProvider(authState));
        context.Services.AddSingleton<IAuthorizationPolicyProvider>(new TestAuthorizationPolicyProvider());
        context.Services.AddSingleton<IAuthorizationService>(new TestAuthorizationService(anAuthenticatedState));
    }
}