using LayeredAPI.Domain.Interfaces.Repositories;
using LayeredAPI.Domain.Models.Entities;
using LayeredAPI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LayeredAPI.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Role> GetRoleByName(string name)
    {
        var role = _context.Roles.Where((Role r) => r.Name.ToLower().Equals(name.ToLower()));
        return await role.FirstOrDefaultAsync();
    }

    public async Task<Role> GetRoleByIdAsync(int id)
    {
        var role = _context.Roles.Where((Role r) => r.Id == id);
        return await role.FirstOrDefaultAsync();
    }
}