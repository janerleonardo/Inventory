
using Inventory.Application.DTOs;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Repositories;

namespace Inventory.Application.Services;

public interface IProductService
{
    Task<ProductDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<IEnumerable<ProductDto>> GetActiveProductsAsync();
    Task<IEnumerable<ProductDto>> GetLowStockProductsAsync();
    Task<IEnumerable<ProductDto>> GetByCategoryAsync(Guid categoryId);
    Task<IEnumerable<ProductDto>> GetBySupplierAsync(Guid supplierId);
    Task<ProductDto> CreateAsync(CreateProductDto dto);
    Task<ProductDto?> UpdateAsync(Guid id, UpdateProductDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<ProductDto?> DecrementStockAsync(Guid id, int amount);
    Task<ProductDto?> IncrementStockAsync(Guid id, int amount);
}