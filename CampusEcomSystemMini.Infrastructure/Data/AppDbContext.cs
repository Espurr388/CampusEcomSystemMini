using CampusEcomSystemMini.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CampusEcomSystemMini.Infrastructure.Data;

public class AppDbContext : DbContext
{
    
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
        
    }

    public DbSet<User> Users {get;set;}

    public DbSet<Preference> Preferences {get;set;}

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Preference>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.MonthlyRentalBudget)
                .HasPrecision(18, 2);

            // Mỗi người dùng chỉ có một bộ vector nhu cầu.
            entity.HasIndex(x => x.UserId).IsUnique();

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}