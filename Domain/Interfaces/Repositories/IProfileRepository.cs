using LayeredAPI.Domain.Models.Entities;

namespace LayeredAPI.Domain.Interfaces.Repositories;

public interface IProfileRepository
{
    Task<User> GetUserProfile(int id);
}