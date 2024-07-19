
using LayeredAPI.Domain.Models.Entities;

namespace LayeredAPI.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> GetUserByEmailAsync(string email);
    Task AddUserAsync(User user);
}