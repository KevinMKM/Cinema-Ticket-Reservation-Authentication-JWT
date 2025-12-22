using Microsoft.AspNetCore.Authorization;

namespace Cinema.Api.Authorization;

public class RequireScopeHandler : AuthorizationHandler<RequireScopeRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        RequireScopeRequirement requirement)
    {
        var scopes = context.User.Claims
            .Where(c => c.Type == "scope")
            .Select(c => c.Value);

        if (scopes.Contains(requirement.Scope))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}