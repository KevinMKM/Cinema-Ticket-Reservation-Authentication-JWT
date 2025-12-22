using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Data;

public class CinemaDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Screening> Screenings { get; set; }

    public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Username).IsUnique();
        });

        SeedAdminUser(modelBuilder);
    }

    private static void SeedAdminUser(ModelBuilder modelBuilder)
    {
        var admin = User.Create("admin", "123456", "Admin",
            new List<string> { "ticket:reserve", "screening:create" });

        modelBuilder.Entity<User>().HasData(admin with { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") });
    }
}