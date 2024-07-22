using LayeredAPI.Domain.Models.Response;

namespace LayeredAPI.Domain.Interfaces.Services;

public interface IRoleService
{
    public Task<RoleResponse> GetRoleById(int id);
}