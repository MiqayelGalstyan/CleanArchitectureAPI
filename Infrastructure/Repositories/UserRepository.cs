using LayeredAPI.Domain.Interfaces.Repositories;
using LayeredAPI.Domain.Models.Entities;
using LayeredAPI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LayeredAPI.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var user = _context.Users
            .Include(u => u.Role)
            .Where(u => u.Email.ToLower().Equals(email.ToLower()));
        return await user.FirstOrDefaultAsync();
    }

    public async Task AddUserAsync(User user)
    {
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}