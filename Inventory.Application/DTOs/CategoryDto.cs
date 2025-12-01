using System.ComponentModel.DataAnnotations;

namespace Inventory.Application.DTOs;

// DTO para respuesta
public record CategoryDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public int ProductCount { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

// DTO para crear categoría
public record CreateCategoryDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
    public string Name { get; init; } = string.Empty;

    [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
    public string Description { get; init; } = string.Empty;
}

// DTO para actualizar categoría
public record UpdateCategoryDto
{
    [StringLength(100, MinimumLength = 3)]
    public string? Name { get; init; }

    [StringLength(500)]
    public string? Description { get; init; }

    public bool? IsActive { get; init; }
}