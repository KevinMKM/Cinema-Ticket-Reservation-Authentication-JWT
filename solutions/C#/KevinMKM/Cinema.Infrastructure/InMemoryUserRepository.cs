using Cinema.Domain;
using Cinema.Domain.Interfaces;

namespace Cinema.Infrastructure
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new()
        {
            new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = "Admin",
                Scopes = new[]{ "tickets:reserve", "screenings:write" }
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "user",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!"),
                Role = "User",
                Scopes = new[]{ "tickets:reserve" }
            }
        };

        public User? GetByUsername(string u) => _users.SingleOrDefault(x => x.Username == u);
    }
}
