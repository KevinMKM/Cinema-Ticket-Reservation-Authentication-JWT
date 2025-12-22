namespace Cinema.Domain.Entities;

public class Ticket
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid ScreeningId { get; set; }
    public int SeatNumber { get; set; }
    public DateTime ReservedAt { get; set; } = DateTime.UtcNow;
}