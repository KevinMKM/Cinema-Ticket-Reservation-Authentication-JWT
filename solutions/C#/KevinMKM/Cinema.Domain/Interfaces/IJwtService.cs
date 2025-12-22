using System.Security.Claims;
using Cinema.Domain.Entities;

namespace Cinema.Domain.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
    Task<ClaimsPrincipal?> ValidateTokenAsync(string token);
}