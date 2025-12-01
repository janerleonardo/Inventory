
using System.ComponentModel.DataAnnotations;

namespace Inventory.Application.DTOs;

// DTO para respuesta
public record SupplierDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string ContactName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public int ProductCount { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

// DTO para crear proveedor
public record CreateSupplierDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 200 caracteres")]
    public string Name { get; init; } = string.Empty;

    [StringLength(100)]
    public string ContactName { get; init; } = string.Empty;

    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(100)]
    public string Email { get; init; } = string.Empty;

    [Phone(ErrorMessage = "Teléfono inválido")]
    [StringLength(20)]
    public string Phone { get; init; } = string.Empty;

    [StringLength(200)]
    public string Address { get; init; } = string.Empty;

    [StringLength(100)]
    public string City { get; init; } = string.Empty;

    [StringLength(100)]
    public string Country { get; init; } = string.Empty;
}

// DTO para actualizar proveedor
public record UpdateSupplierDto
{
    [StringLength(200, MinimumLength = 3)]
    public string? Name { get; init; }

    [StringLength(100)]
    public string? ContactName { get; init; }

    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; init; }

    [Phone]
    [StringLength(20)]
    public string? Phone { get; init; }

    [StringLength(200)]
    public string? Address { get; init; }

    [StringLength(100)]
    public string? City { get; init; }

    [StringLength(100)]
    public string? Country { get; init; }

    public bool? IsActive { get; init; }
}