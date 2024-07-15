using LayeredAPI.Domain.Models.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace LayeredAPI.Infrastructure.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public  DbSet<User> Users { get; set; }
    
}