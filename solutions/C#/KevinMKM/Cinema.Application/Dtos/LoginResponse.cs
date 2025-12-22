namespace Cinema.Application.Dtos;

public record LoginResponse(string AccessToken, DateTime ExpiresAt);