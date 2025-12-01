namespace Inventory.Domain.Entities;

public  class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int StockMinimal { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


    public bool HasLowStock() => Stock < StockMinimal;

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