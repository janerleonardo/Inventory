using Inventory.Application.DTOs;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Repositories;

namespace Inventory.Application.Services;

public interface ISupplierService
{
    Task<SupplierDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<SupplierDto>> GetAllAsync();
    Task<IEnumerable<SupplierDto>> GetActiveAsync();
    Task<SupplierDto> CreateAsync(CreateSupplierDto dto);
    Task<SupplierDto?> UpdateAsync(Guid id, UpdateSupplierDto dto);
    Task<bool> DeleteAsync(Guid id);
}

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _repository;

    public SupplierService(ISupplierRepository repository)
    {
        _repository = repository;
    }

    public async Task<SupplierDto?> GetByIdAsync(Guid id)
    {
        var supplier = await _repository.GetByIdAsync(id);
        return supplier != null ? MapToDto(supplier) : null;
    }

    public async Task<IEnumerable<SupplierDto>> GetAllAsync()
    {
        var suppliers = await _repository.GetAllAsync();
        return suppliers.Select(MapToDto);
    }

    public async Task<IEnumerable<SupplierDto>> GetActiveAsync()
    {
        var suppliers = await _repository.GetActiveAsync();
        return suppliers.Select(MapToDto);
    }

    public async Task<SupplierDto> CreateAsync(CreateSupplierDto dto)
    {
        var supplier = new Supplier
        {
            Name = dto.Name,
            ContactName = dto.ContactName,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = dto.Address,
            City = dto.City,
            Country = dto.Country,
            IsActive = true
        };

        var created = await _repository.CreateAsync(supplier);
        return MapToDto(created);
    }

    public async Task<SupplierDto?> UpdateAsync(Guid id, UpdateSupplierDto dto)
    {
        var supplier = await _repository.GetByIdAsync(id);
        if (supplier == null) return null;

        if (dto.Name != null) supplier.Name = dto.Name;
        if (dto.ContactName != null) supplier.ContactName = dto.ContactName;
        if (dto.Email != null) supplier.Email = dto.Email;
        if (dto.Phone != null) supplier.Phone = dto.Phone;
        if (dto.Address != null) supplier.Address = dto.Address;
        if (dto.City != null) supplier.City = dto.City;
        if (dto.Country != null) supplier.Country = dto.Country;
        if (dto.IsActive.HasValue) supplier.IsActive = dto.IsActive.Value;

        supplier.UpdatedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(supplier);
        return updated != null ? MapToDto(updated) : null;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        // Verificar si tiene productos asociados
        var hasProducts = await _repository.HasProductsAsync(id);
        if (hasProducts)
        {
            throw new InvalidOperationException(
                "No se puede eliminar el proveedor porque tiene productos asociados");
        }

        return await _repository.DeleteAsync(id);
    }

    private static SupplierDto MapToDto(Supplier supplier)
    {
        return new SupplierDto
        {
            Id = supplier.Id,
            Name = supplier.Name,
            ContactName = supplier.ContactName,
            Email = supplier.Email,
            Phone = supplier.Phone,
            Address = supplier.Address,
            City = supplier.City,
            Country = supplier.Country,
            IsActive = supplier.IsActive,
            ProductCount = supplier.Products?.Count ?? 0,
            CreatedAt = supplier.CreatedAt,
            UpdatedAt = supplier.UpdatedAt
        };
    }
}