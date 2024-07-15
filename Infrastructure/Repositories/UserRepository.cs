using Domain.Interfaces.Repositories.UserRepository;
using LayeredAPI.Domain.Models.Entities.User;
using LayeredAPI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LayeredAPI.Infrastructure.Repositories.UserRepository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        IQueryable<User> user = _context.Users.Where(u => u.Email.ToLower().Equals(email.ToLower()));
        return await user.FirstOrDefaultAsync();
    }

    public async Task AddUserAsync(User user)
    {
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}