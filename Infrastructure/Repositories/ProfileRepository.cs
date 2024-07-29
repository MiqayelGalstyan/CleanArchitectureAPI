using LayeredAPI.Domain.Interfaces.Repositories;
using LayeredAPI.Domain.Models.Entities;
using LayeredAPI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LayeredAPI.Infrastructure.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly ApplicationDbContext _context;

    public ProfileRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<User> GetUserProfile(int id)
    {
        var user = _context.Users
            .Include(u => u.Role)
            .Where((User u) => u.Id.Equals(id));

        return await user.FirstOrDefaultAsync();
    }
}