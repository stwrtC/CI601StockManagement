using GenericStockManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class StockDbContextFactory : IDesignTimeDbContextFactory<StockDbContext>
{
    public StockDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<StockDbContext>();

        optionsBuilder.UseSqlServer("Server=tcp:ci601stockdb.database.windows.net,1433;Initial Catalog=CI601StockDB;Persist Security Info=False;User ID=cs1657;Password=Pabfuv9b;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        return new StockDbContext(optionsBuilder.Options);
    }
}
