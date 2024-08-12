
using LayeredAPI.Domain.Models.Entities;
using LayeredAPI.Domain.Models.Request;

namespace LayeredAPI.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> GetUserByEmailAsync(string email);
    Task AddUserAsync(User user);
    
    Task<List<User>> GetUsers(GetUsersRequest getUsersRequest);

    Task<User> GetUser(int id);

    Task UpdateUserAsync(User user);

    Task<int> GetUsersCount(string searchQuery);

    Task<bool> DeleteUser(User user);
}