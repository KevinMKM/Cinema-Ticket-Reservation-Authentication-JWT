namespace Cinema.Domain.Entities;

public class Screening
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string MovieTitle { get; set; } = string.Empty;
    public DateTime ShowTime { get; set; }
    public int TotalSeats { get; set; }
    public List<string> AvailableSeats { get; set; } = new();
}