using InnowiseFridge_project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace InnowiseFridge_project.Data;

public class DataContext : DbContext
{
    public DbSet<Fridge> Fridges { get; set; } = null!;
    public DbSet<FridgeModel> FridgeModels { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<FridgeProduct> FridgeProducts { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        // var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;

        // if (databaseCreator != null)
        // {
        //     if (!databaseCreator.CanConnect()) databaseCreator.Create();
        //     if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
        // }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Fridge>()
            .HasMany(f => f.Products)
            .WithMany(p => p.Fridges)
            .UsingEntity<FridgeProduct>(
                j => j
                    .HasOne(fp => fp.Product)
                    .WithMany()
                    .HasForeignKey(fp => fp.ProductId),
                j => j
                    .HasOne(fp => fp.Fridge)
                    .WithMany()
                    .HasForeignKey(fp => fp.FridgeId),
        j =>        
                {
                    j.HasKey(t => new { t.FridgeId, t.ProductId });
                    j.ToTable("FridgeProducts");
                });

        modelBuilder.Entity<FridgeModel>()
            .HasMany(f => f.Fridges)
            .WithOne(f => f.FridgeModel)
            .HasForeignKey(f => f.FridgeModelId)
            .IsRequired();

        base.OnModelCreating(modelBuilder);
    }
}