using System.Reflection;
using Microsoft.EntityFrameworkCore;
using mPass.Domain.Entities;

namespace mPass.Persistence;

public class MPassDbContext(DbContextOptions<MPassDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}