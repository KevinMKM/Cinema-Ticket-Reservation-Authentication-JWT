using Cinema.Application.Dtos;

namespace Cinema.Application.Interfaces;

public interface ILoginUseCase
{
    Task<Result<LoginResponse>> ExecuteAsync(LoginRequest request);
}