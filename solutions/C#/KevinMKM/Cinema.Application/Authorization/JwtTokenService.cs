using System.IdentityModel.Tokens.Jwt;
using Cinema.Domain;
using System.Security.Claims;
using System.Text;
using Cinema.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Cinema.Application.Authorization;

public class JwtTokenService : IJwtTokenService
{
    public string GenerateToken(User user)
    {
        var key = Environment.GetEnvironmentVariable("JWT__SigningKey")!;
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("scope", string.Join(' ', user.Scopes)),
            new Claim(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var token = new JwtSecurityToken(
            "cinema-auth",
            "cinema-api",
            claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}