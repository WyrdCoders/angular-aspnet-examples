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

    /* Fluent API Example
     * Fluent API configurations override any existing EF Core convention or data annotations
     * applied to entity classes and properties.
     */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<City>().ToTable("Cities");
        modelBuilder.Entity<City>().HasKey(x => x.Id);
        modelBuilder.Entity<City>().Property(x => x.Id).IsRequired();
        modelBuilder.Entity<City>().Property(x => x.Lat).HasColumnType("decimal(7,4)");
        modelBuilder.Entity<City>().Property(x => x.Lon).HasColumnType("decimal(7,4)");

        modelBuilder.Entity<Country>().ToTable("Countries");
        modelBuilder.Entity<Country>().HasKey(x => x.Id);
        modelBuilder.Entity<Country>().Property(x => x.Id).IsRequired();
        modelBuilder.Entity<City>().HasOne(x => x.Country).WithMany(y => y.Cities).HasForeignKey(x => x.CountryId);
    }
}
