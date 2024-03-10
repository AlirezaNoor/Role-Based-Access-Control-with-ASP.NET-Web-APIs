using AccessControl.Entity.Users;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Context;

public class MyContext:DbContext
{
    public MyContext(DbContextOptions<MyContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }


  
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Roles)
                .HasConversion(
                    u => string.Join(',', u),
                    u => u.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
        }
  
}