using Cinema.Api.Dtos;
using Cinema.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtTokenService _jwt;

    public AuthController(IUserRepository users, IPasswordHasher hasher, IJwtTokenService jwt)
    {
        _users = users;
        _hasher = hasher;
        _jwt = jwt;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        var user = _users.GetByUsername(dto.Username);
        if (user == null || !_hasher.Verify(dto.Password, user.PasswordHash))
            return Unauthorized();

        return Ok(new { accessToken = _jwt.GenerateToken(user), tokenType = "Bearer" });
    }
}