using Inventory.Domain.Entities;
namespace Inventory.Infrastructure.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(Guid id);
    Task<IEnumerable<Category>> GetAllAsync();
    Task<IEnumerable<Category>> GetActiveAsync();
    Task<Category> CreateAsync(Category category);
    Task<Category?> UpdateAsync(Category category);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> HasProductsAsync(Guid id);
}

