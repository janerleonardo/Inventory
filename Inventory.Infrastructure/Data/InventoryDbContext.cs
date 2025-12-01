using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Data;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");
            
            entity.HasKey(p => p.Id);
            
            entity.Property(p => p.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            entity.Property(p => p.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(p => p.Price)
                .HasColumnName("price")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            
            entity.Property(p => p.Stock)
                .HasColumnName("stock")
                .IsRequired();
            
            entity.Property(p => p.StockMinimal)
                .HasColumnName("stock_minimal")
                .IsRequired();
            
            entity.Property(p => p.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);
            
            entity.Property(p => p.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();
            
            entity.Property(p => p.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            // Índices
            entity.HasIndex(p => p.Name);
            entity.HasIndex(p => p.IsActive);
        });
    }
}