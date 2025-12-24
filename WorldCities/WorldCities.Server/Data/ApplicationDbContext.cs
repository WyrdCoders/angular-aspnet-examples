using Microsoft.EntityFrameworkCore;
using WorldCities.Server.Data.Models;

namespace WorldCities.Server.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() : base()
    {
    }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<City> Cities => Set<City>();
    public DbSet<Country> Countries => Set<Country>();

    /* Fluent API with EntityTypeConfiguration classes Example
     * Fluent API configurations override any existing EF Core convention or data annotations
     * applied to entity classes and properties.
     */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Add the EntityTypeConfiguration classes
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
