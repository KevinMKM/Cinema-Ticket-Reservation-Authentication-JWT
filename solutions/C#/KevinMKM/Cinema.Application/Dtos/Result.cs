namespace Cinema.Application.Dtos;

public record Result<T>(T? Data, string? Error);