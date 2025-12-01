

using Inventory.Domain.Entities;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly InventoryDbContext _context;

    public ProductRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetLowStockProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Where(p => p.IsActive && p.Stock < p.StockMinimal)
            .OrderBy(p => p.Stock)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Where(p => p.CategoryId == categoryId && p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetBySupplierAsync(Guid supplierId)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Where(p => p.SupplierId == supplierId && p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Product> CreateAsync(Product product)
    {
        product.Id = Guid.NewGuid();
        product.CreatedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Cargar las relaciones después de crear
        await _context.Entry(product).Reference(p => p.Category).LoadAsync();
        await _context.Entry(product).Reference(p => p.Supplier).LoadAsync();
        
        return product;
    }

    public async Task<Product?> UpdateAsync(Product product)
    {
        var existing = await GetByIdAsync(product.Id);
        if (existing == null) return null;

        product.UpdatedAt = DateTime.UtcNow;
        _context.Entry(existing).CurrentValues.SetValues(product);
        await _context.SaveChangesAsync();

        // Recargar las relaciones
        await _context.Entry(existing).Reference(p => p.Category).LoadAsync();
        await _context.Entry(existing).Reference(p => p.Supplier).LoadAsync();
        
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Products.AnyAsync(p => p.Id == id);
    }
}