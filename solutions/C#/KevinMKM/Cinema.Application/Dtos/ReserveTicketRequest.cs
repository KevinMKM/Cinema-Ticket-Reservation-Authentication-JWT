namespace Cinema.Application.Dtos;

public record ReserveTicketRequest(Guid ScreeningId, int SeatNumber);