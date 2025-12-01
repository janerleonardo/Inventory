using System.ComponentModel.DataAnnotations;

namespace Inventory.Application.DTOs;

// DTO para respuesta
public record ProductDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public int Stock { get; init; }
    public int StockMinimal { get; init; }
    public bool IsActive { get; init; }
    public bool HasLowStock { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

// DTO para crear producto
public record CreateProductDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 200 caracteres")]
    public string Name { get; init; } = string.Empty;

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a 0")]
    public decimal Price { get; init; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser mayor o igual a 0")]
    public int Stock { get; init; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "El stock mínimo debe ser mayor o igual a 0")]
    public int StockMinimal { get; init; }
}

// DTO para actualizar producto
public record UpdateProductDto
{
    [StringLength(200, MinimumLength = 3)]
    public string? Name { get; init; }

    [Range(0, double.MaxValue)]
    public decimal? Price { get; init; }

    [Range(0, int.MaxValue)]
    public int? Stock { get; init; }

    [Range(0, int.MaxValue)]
    public int? StockMinimal { get; init; }

    public bool? IsActive { get; init; }
}

// DTO para ajustar stock
public record AdjustStockDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
    public int Amount { get; init; }
}