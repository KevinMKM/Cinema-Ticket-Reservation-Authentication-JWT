namespace Cinema.Application.Dtos;

public record CreateScreeningRequest(
    string MovieTitle,
    DateTime ShowTime,
    int TotalSeats);