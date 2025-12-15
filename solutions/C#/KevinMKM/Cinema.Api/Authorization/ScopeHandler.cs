using Microsoft.AspNetCore.Authorization;

namespace Cinema.Api.Authorization;

public sealed class ScopeHandler : AuthorizationHandler<ScopeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeRequirement requirement)
    {
        var scopeClaim = context.User.FindFirst("scope")?.Value;
        if (scopeClaim?.Split(' ').Contains(requirement.Scope) == true)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}