using LayeredAPI.Domain.Models.Entities;

namespace LayeredAPI.Domain.Interfaces.Repositories;

public interface IRoleRepository
{
    Task<Role> GetRoleByName(string name);
    Task<Role> GetRoleByIdAsync(int id);
}