
using Inventory.Application.DTOs;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Repositories;

namespace Inventory.Application.Services;
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISupplierRepository _supplierRepository;

    public ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        ISupplierRepository supplierRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _supplierRepository = supplierRepository;
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product != null ? MapToDto(product) : null;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductDto>> GetActiveProductsAsync()
    {
        var products = await _productRepository.GetActiveProductsAsync();
        return products.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductDto>> GetLowStockProductsAsync()
    {
        var products = await _productRepository.GetLowStockProductsAsync();
        return products.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductDto>> GetByCategoryAsync(Guid categoryId)
    {
        var products = await _productRepository.GetByCategoryAsync(categoryId);
        return products.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductDto>> GetBySupplierAsync(Guid supplierId)
    {
        var products = await _productRepository.GetBySupplierAsync(supplierId);
        return products.Select(MapToDto);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        // Validar que exista la categoría
        if (!await _categoryRepository.ExistsAsync(dto.CategoryId))
        {
            throw new InvalidOperationException("La categoría especificada no existe");
        }

        // Validar que exista el proveedor
        if (!await _supplierRepository.ExistsAsync(dto.SupplierId))
        {
            throw new InvalidOperationException("El proveedor especificado no existe");
        }

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            StockMinimal = dto.StockMinimal,
            CategoryId = dto.CategoryId,
            SupplierId = dto.SupplierId,
            IsActive = true
        };

        var created = await _productRepository.CreateAsync(product);
        return MapToDto(created);
    }

    public async Task<ProductDto?> UpdateAsync(Guid id, UpdateProductDto dto)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return null;

        // Validar categoría si se está actualizando
        if (dto.CategoryId.HasValue && 
            !await _categoryRepository.ExistsAsync(dto.CategoryId.Value))
        {
            throw new InvalidOperationException("La categoría especificada no existe");
        }

        // Validar proveedor si se está actualizando
        if (dto.SupplierId.HasValue && 
            !await _supplierRepository.ExistsAsync(dto.SupplierId.Value))
        {
            throw new InvalidOperationException("El proveedor especificado no existe");
        }

        if (dto.Name != null) product.Name = dto.Name;
        if (dto.Description != null) product.Description = dto.Description;
        if (dto.Price.HasValue) product.UpdatePrice(dto.Price.Value);
        if (dto.Stock.HasValue) product.Stock = dto.Stock.Value;
        if (dto.StockMinimal.HasValue) product.StockMinimal = dto.StockMinimal.Value;
        if (dto.CategoryId.HasValue) product.CategoryId = dto.CategoryId.Value;
        if (dto.SupplierId.HasValue) product.SupplierId = dto.SupplierId.Value;
        if (dto.IsActive.HasValue) product.IsActive = dto.IsActive.Value;

        product.UpdatedAt = DateTime.UtcNow;

        var updated = await _productRepository.UpdateAsync(product);
        return updated != null ? MapToDto(updated) : null;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _productRepository.DeleteAsync(id);
    }

    public async Task<ProductDto?> DecrementStockAsync(Guid id, int amount)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return null;

        product.DecrementStock(amount);
        var updated = await _productRepository.UpdateAsync(product);
        
        return updated != null ? MapToDto(updated) : null;
    }

    public async Task<ProductDto?> IncrementStockAsync(Guid id, int amount)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return null;

        product.IncrementStock(amount);
        var updated = await _productRepository.UpdateAsync(product);
        
        return updated != null ? MapToDto(updated) : null;
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            StockMinimal = product.StockMinimal,
            IsActive = product.IsActive,
            HasLowStock = product.HasLowStock(),
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty,
            SupplierId = product.SupplierId,
            SupplierName = product.Supplier?.Name ?? string.Empty,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }

    public async Task<int> GetCountProductsAsync()
    {
        return await _productRepository.GetCountProductsAsync();
    }
}