using Inventory.Application.DTOs;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Repositories;

namespace Inventory.Application.Services;

public interface ICategoryService
{
    Task<CategoryDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<CategoryDto>> GetAllAsync();
    Task<IEnumerable<CategoryDto>> GetActiveAsync();
    Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
    Task<CategoryDto?> UpdateAsync(Guid id, UpdateCategoryDto dto);
    Task<bool> DeleteAsync(Guid id);
}

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<CategoryDto?> GetByIdAsync(Guid id)
    {
        var category = await _repository.GetByIdAsync(id);
        return category != null ? MapToDto(category) : null;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var categories = await _repository.GetAllAsync();
        return categories.Select(MapToDto);
    }

    public async Task<IEnumerable<CategoryDto>> GetActiveAsync()
    {
        var categories = await _repository.GetActiveAsync();
        return categories.Select(MapToDto);
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            IsActive = true
        };

        var created = await _repository.CreateAsync(category);
        return MapToDto(created);
    }

    public async Task<CategoryDto?> UpdateAsync(Guid id, UpdateCategoryDto dto)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null) return null;

        if (dto.Name != null) category.Name = dto.Name;
        if (dto.Description != null) category.Description = dto.Description;
        if (dto.IsActive.HasValue) category.IsActive = dto.IsActive.Value;

        category.UpdatedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(category);
        return updated != null ? MapToDto(updated) : null;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        // Verificar si tiene productos asociados
        var hasProducts = await _repository.HasProductsAsync(id);
        if (hasProducts)
        {
            throw new InvalidOperationException(
                "No se puede eliminar la categoría porque tiene productos asociados");
        }

        return await _repository.DeleteAsync(id);
    }

    private static CategoryDto MapToDto(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive,
            ProductCount = category.Products?.Count ?? 0,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }
}