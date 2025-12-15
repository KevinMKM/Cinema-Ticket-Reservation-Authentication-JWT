namespace Cinema.Domain.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}