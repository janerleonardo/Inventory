
namespace Inventory.Domain.Entities;

public class Supplier
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Relación con productos
    public ICollection<Product> Products { get; set; } = new List<Product>();

    // Método para actualizar información de contacto
    public void UpdateContactInfo(string email, string phone, string contactName)
    {
        if (!string.IsNullOrWhiteSpace(email))
            Email = email;
        
        if (!string.IsNullOrWhiteSpace(phone))
            Phone = phone;
        
        if (!string.IsNullOrWhiteSpace(contactName))
            ContactName = contactName;

        UpdatedAt = DateTime.UtcNow;
    }

    // Método para activar/desactivar proveedor
    public void ToggleActive()
    {
        IsActive = !IsActive;
        UpdatedAt = DateTime.UtcNow;
    }
}