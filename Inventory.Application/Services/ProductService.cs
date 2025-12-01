
using Inventory.Application.DTOs;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Repositories;

namespace Inventory.Application.Services;
public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);
        return product != null ? MapToDto(product) : null;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();
        return products.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductDto>> GetActiveProductsAsync()
    {
        var products = await _repository.GetActiveProductsAsync();
        return products.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductDto>> GetLowStockProductsAsync()
    {
        var products = await _repository.GetLowStockProductsAsync();
        return products.Select(MapToDto);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            Stock = dto.Stock,
            StockMinimal = dto.StockMinimal,
            IsActive = true
        };

        var created = await _repository.CreateAsync(product);
        return MapToDto(created);
    }

    public async Task<ProductDto?> UpdateAsync(Guid id, UpdateProductDto dto)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) return null;

        if (dto.Name != null) product.Name = dto.Name;
        if (dto.Price.HasValue) product.UpdatePrice(dto.Price.Value);
        if (dto.Stock.HasValue) product.Stock = dto.Stock.Value;
        if (dto.StockMinimal.HasValue) product.StockMinimal = dto.StockMinimal.Value;
        if (dto.IsActive.HasValue) product.IsActive = dto.IsActive.Value;

        product.UpdatedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(product);
        return updated != null ? MapToDto(updated) : null;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<ProductDto?> DecrementStockAsync(Guid id, int amount)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) return null;

        product.DecrementStock(amount);
        var updated = await _repository.UpdateAsync(product);
        
        return updated != null ? MapToDto(updated) : null;
    }

    public async Task<ProductDto?> IncrementStockAsync(Guid id, int amount)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) return null;

        product.IncrementStock(amount);
        var updated = await _repository.UpdateAsync(product);
        
        return updated != null ? MapToDto(updated) : null;
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock,
            StockMinimal = product.StockMinimal,
            IsActive = product.IsActive,
            HasLowStock = product.HasLowStock(),
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }
}