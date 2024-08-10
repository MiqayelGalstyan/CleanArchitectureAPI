using LayeredAPI.Domain.Interfaces.Repositories;
using LayeredAPI.Domain.Interfaces.Services;
using LayeredAPI.Domain.Mappers;
using LayeredAPI.Domain.Models.Response;

namespace LayeredAPI.Application.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly RoleMapper _mapper;

    public RoleService(IRoleRepository roleRepository,RoleMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<RoleResponse> GetRoleById(int id)
    {
        var role = await _roleRepository.GetRoleByIdAsync(id);
        
        return _mapper.MapRole(role);
    }
}