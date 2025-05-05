using GenericStockManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class StockDbContextFactory : IDesignTimeDbContextFactory<StockDbContext>
{
    public StockDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<StockDbContext>();

        //  Local SQL Server connection string (adjust server name if needed)
        optionsBuilder.UseSqlServer("Server=localhost;Database=StockDb;Trusted_Connection=True;TrustServerCertificate=True;");

        return new StockDbContext(optionsBuilder.Options);
    }
}
