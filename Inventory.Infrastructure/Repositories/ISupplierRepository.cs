using Inventory.Domain.Entities;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories;

public interface ISupplierRepository
{
    Task<Supplier?> GetByIdAsync(Guid id);
    Task<IEnumerable<Supplier>> GetAllAsync();
    Task<IEnumerable<Supplier>> GetActiveAsync();
    Task<Supplier> CreateAsync(Supplier supplier);
    Task<Supplier?> UpdateAsync(Supplier supplier);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> HasProductsAsync(Guid id);
    
}

public class SupplierRepository : ISupplierRepository
{
    private readonly InventoryDbContext _context;

    public SupplierRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<Supplier?> GetByIdAsync(Guid id)
    {
        return await _context.Suppliers
            .Include(s => s.Products)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Supplier>> GetAllAsync()
    {
        return await _context.Suppliers
            .Include(s => s.Products)
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Supplier>> GetActiveAsync()
    {
        return await _context.Suppliers
            .Include(s => s.Products)
            .Where(s => s.IsActive)
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<Supplier> CreateAsync(Supplier supplier)
    {
        supplier.Id = Guid.NewGuid();
        supplier.CreatedAt = DateTime.UtcNow;
        supplier.UpdatedAt = DateTime.UtcNow;

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
        
        return supplier;
    }

    public async Task<Supplier?> UpdateAsync(Supplier supplier)
    {
        var existing = await _context.Suppliers.FindAsync(supplier.Id);
        if (existing == null) return null;

        supplier.UpdatedAt = DateTime.UtcNow;
        _context.Entry(existing).CurrentValues.SetValues(supplier);
        await _context.SaveChangesAsync();
        
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var supplier = await GetByIdAsync(id);
        if (supplier == null) return false;

        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Suppliers.AnyAsync(s => s.Id == id);
    }

    public async Task<bool> HasProductsAsync(Guid id)
    {
        return await _context.Products.AnyAsync(p => p.SupplierId == id);
    }
}