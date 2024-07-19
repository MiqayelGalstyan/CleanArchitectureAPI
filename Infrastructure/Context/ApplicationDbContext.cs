using LayeredAPI.Domain.Models.Entities;
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