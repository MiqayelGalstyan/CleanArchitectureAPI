
using LayeredAPI.Domain.Models.Entities;
using LayeredAPI.Domain.Models.Response;

namespace LayeredAPI.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> GetUserByEmailAsync(string email);
    Task AddUserAsync(User user);
    
    Task<List<User>> GetUsers();

    Task<User> GetUser(int id);
}