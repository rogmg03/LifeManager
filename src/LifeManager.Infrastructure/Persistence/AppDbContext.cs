using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Cycle 1
    public DbSet<User> Users => Set<User>();
    public DbSet<UserSettings> UserSettings => Set<UserSettings>();

    // Cycle 2
    public DbSet<Client> Clients => Set<Client>();

    // Cycle 3
    public DbSet<Project> Projects => Set<Project>();

    // Cycle 4
    public DbSet<Phase> Phases => Set<Phase>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
