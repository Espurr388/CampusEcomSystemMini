using CampusEcomSystemMini.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CampusEcomSystemMini.Infrastructure.Data;

public class AppDbContext : DbContext
{
    
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
        
    }

    public DbSet<User> Users {get;set;}

} 