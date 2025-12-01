using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Inventory.Infrastructure.Data;

public class InventoryDbContextFactory : IDesignTimeDbContextFactory<InventoryDbContext>
{
    public InventoryDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();

        // Pon aquí la cadena de conexión de desarrollo
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5433;Database=InventoryDB;Username=postgres;Password=Yeshua"
        );

        return new InventoryDbContext(optionsBuilder.Options);
    }
}