namespace Inventory.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int StockMinimal { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public Guid CategoryId { get; set; }
    public Guid SupplierId { get; set; }

    // Navigation Properties
    public Category Category { get; set; } = null!;
    public Supplier Supplier { get; set; } = null!;

    // Método para verificar stock bajo
    public bool HasLowStock() => Stock < StockMinimal;

    // Método para decrementar stock
    public void DecrementStock(int amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("La cantidad debe ser mayor a 0", nameof(amount));
        }

        if (Stock < amount)
        {
            throw new InvalidOperationException(
                $"Stock insuficiente. Stock actual: {Stock}, Cantidad solicitada: {amount}");
        }

        Stock -= amount;
        UpdatedAt = DateTime.UtcNow;
    }

    // Método para incrementar stock
    public void IncrementStock(int amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("La cantidad debe ser mayor a 0", nameof(amount));
        }

        Stock += amount;
        UpdatedAt = DateTime.UtcNow;
    }

    // Método para actualizar precio
    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
        {
            throw new ArgumentException("El precio no puede ser negativo", nameof(newPrice));
        }

        Price = newPrice;
        UpdatedAt = DateTime.UtcNow;
    }
}