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
    public DbSet<Category> Categories { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("categories");
            
            entity.HasKey(c => c.Id);
            
            entity.Property(c => c.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            entity.Property(c => c.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(c => c.Description)
                .HasColumnName("description")
                .HasMaxLength(500);
            
            entity.Property(c => c.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);
            
            entity.Property(c => c.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();
            
            entity.Property(c => c.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            entity.HasIndex(c => c.Name).IsUnique();
        });

        // Configuración de Supplier
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("suppliers");
            
            entity.HasKey(s => s.Id);
            
            entity.Property(s => s.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            entity.Property(s => s.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(s => s.ContactName)
                .HasColumnName("contact_name")
                .HasMaxLength(100);
            
            entity.Property(s => s.Email)
                .HasColumnName("email")
                .HasMaxLength(100);
            
            entity.Property(s => s.Phone)
                .HasColumnName("phone")
                .HasMaxLength(20);
            
            entity.Property(s => s.Address)
                .HasColumnName("address")
                .HasMaxLength(200);
            
            entity.Property(s => s.City)
                .HasColumnName("city")
                .HasMaxLength(100);
            
            entity.Property(s => s.Country)
                .HasColumnName("country")
                .HasMaxLength(100);
            
            entity.Property(s => s.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);
            
            entity.Property(s => s.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();
            
            entity.Property(s => s.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            entity.HasIndex(s => s.Email);
        });

        // Configuración de Product
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
            
            entity.Property(p => p.Description)
                .HasColumnName("description")
                .HasMaxLength(1000);
            
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

            entity.Property(p => p.CategoryId)
                .HasColumnName("category_id")
                .IsRequired();

            entity.Property(p => p.SupplierId)
                .HasColumnName("supplier_id")
                .IsRequired();

            // Relaciones
            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            entity.HasIndex(p => p.Name);
            entity.HasIndex(p => p.IsActive);
            entity.HasIndex(p => p.CategoryId);
            entity.HasIndex(p => p.SupplierId);
        });
    }
}