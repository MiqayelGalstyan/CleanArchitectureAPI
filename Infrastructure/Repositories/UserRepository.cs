using LayeredAPI.Domain.Interfaces.Repositories;
using LayeredAPI.Domain.Models.Entities;
using LayeredAPI.Domain.Models.Request;
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
    
    
    public async Task<User> GetUser(int id)
    {
        var user = _context.Users.Where((User u) => u.Id.Equals(id)).Include(u => u.Role);
        return await user.FirstOrDefaultAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetUsers(GetUsersRequest getUsersRequest)
    {
        return await _context.Users
            .Include(u => u.Role)
            .Where(u => string.IsNullOrEmpty(getUsersRequest.SearchQuery) || 
                        (u.FirstName.ToLower().Contains(getUsersRequest.SearchQuery.ToLower()) || 
                         u.LastName.ToLower().Contains(getUsersRequest.SearchQuery.ToLower())))
            .Skip((getUsersRequest.Page - 1) * getUsersRequest.Limit) 
            .Take(getUsersRequest.Limit)
            .ToListAsync();
    }
    
    public async Task<int> GetUsersCount(string searchQuery)
    {
        return await _context.Users
            .CountAsync(u => string.IsNullOrEmpty(searchQuery) || (u.FirstName.ToLower().Contains(searchQuery.ToLower()) || u.LastName.ToLower().Contains(searchQuery.ToLower())));
    }
}