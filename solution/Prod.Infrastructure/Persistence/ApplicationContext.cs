using Microsoft.EntityFrameworkCore;
using Prod.Domain.Entities;

namespace Prod.Infrastructure.Persistence;

public class ApplicationContext : DbContext
{
    public DbSet<Country> Countries { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Relation> Relations { get; set; }
    public DbSet<Post> Posts { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.Migrate();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Country>()
            .ToTable("countries",
            t => t.ExcludeFromMigrations());

        modelBuilder
            .Entity<Relation>()
            .HasOne(t => t.User);
        
        modelBuilder
            .Entity<Relation>()
            .HasOne(t => t.Friend);

        modelBuilder
            .Entity<User>()
            .HasMany(t => t.Posts)
            .WithOne(t => t.Author);
    }
}