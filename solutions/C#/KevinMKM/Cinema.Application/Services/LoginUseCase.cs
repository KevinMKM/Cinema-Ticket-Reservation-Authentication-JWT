using Cinema.Application.Dtos;
using Cinema.Application.Interfaces;
using Cinema.Domain.Interfaces;

namespace Cinema.Application.Services;

public class LoginUseCase : ILoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public LoginUseCase(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<Result<LoginResponse>> ExecuteAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);
        if (user?.VerifyPassword(request.Password) != true)
            return Result<LoginResponse>.Failure("Invalid credentials");

        var token = _jwtService.GenerateToken(user);
        return Result<LoginResponse>.Success(new LoginResponse(token, DateTime.UtcNow.AddMinutes(30)));
    }
}