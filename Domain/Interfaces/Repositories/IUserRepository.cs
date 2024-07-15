
using LayeredAPI.Domain.Models.Entities.User;

namespace Domain.Interfaces.Repositories.UserRepository;

public interface IUserRepository
{
    Task<User> GetUserByEmailAsync(string email);
    Task AddUserAsync(User user);
}