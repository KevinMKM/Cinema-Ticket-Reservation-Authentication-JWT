using Microsoft.AspNetCore.Authorization;

namespace Cinema.Api.Authorization;

public class RequireScopeRequirement : IAuthorizationRequirement
{
    public string Scope { get; }

    public RequireScopeRequirement(string scope) => Scope = scope;
}