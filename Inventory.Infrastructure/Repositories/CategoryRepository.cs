
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly InventoryDbContext _context;

    public CategoryRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories
            .Include(c => c.Products)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetActiveAsync()
    {
        return await _context.Categories
            .Include(c => c.Products)
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Category> CreateAsync(Category category)
    {
        category.Id = Guid.NewGuid();
        category.CreatedAt = DateTime.UtcNow;
        category.UpdatedAt = DateTime.UtcNow;

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        
        return category;
    }

    public async Task<Category?> UpdateAsync(Category category)
    {
        var existing = await _context.Categories.FindAsync(category.Id);
        if (existing == null) return null;

        category.UpdatedAt = DateTime.UtcNow;
        _context.Entry(existing).CurrentValues.SetValues(category);
        await _context.SaveChangesAsync();
        
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var category = await GetByIdAsync(id);
        if (category == null) return false;

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Categories.AnyAsync(c => c.Id == id);
    }

    public async Task<bool> HasProductsAsync(Guid id)
    {
        return await _context.Products.AnyAsync(p => p.CategoryId == id);
    }
}