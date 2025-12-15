namespace Cinema.Domain.Interfaces;

public interface IUserRepository
{
    User? GetByUsername(string user);
}