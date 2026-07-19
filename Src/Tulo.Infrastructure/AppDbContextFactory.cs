using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tulo.Infrastructure;
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // Simplest way: fixed connection for local migrations
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=InvoiceManagerDb; Trusted_Connection=True;");

        return new AppDbContext(optionsBuilder.Options);
    }
}
