using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Runtime.CompilerServices;
using Tulo.Application.Interfaces.Services;
using Tulo.Application.Interfaces.UnitOfWorks;
using Tulo.Infrastructure;

namespace TestList;

public class TestSystem
{
    public IUnitOfWorkFactory UnitOfWorkFactory = null!;
    public ISellerService SellerService = null!;
    public ICustomerService CustomerService = null!;
    private const string RequiredTestDbName = "InvoiceManagerDb-Tests";

    private readonly DbContextOptions<AppDbContext> _dbContextOptions;

    public TestSystem()
    {
        var config = ObtainConfiguration();
        var connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not configured.");

        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(connectionString).Options;

        EnsureTestDatabase();
    }

    public AppDbContext CreateDbContext() => new(_dbContextOptions);

    public IServiceScope CreateTestServiceScope()
    {
        var host = CreateTestHostInstance();
        return host.Services.CreateScope();
    }

    public void SetRequiredServices(IServiceScope scope)
    {
        UnitOfWorkFactory = scope.ServiceProvider.GetRequiredService<IUnitOfWorkFactory>();
        SellerService = scope.ServiceProvider.GetRequiredService<ISellerService>();
        CustomerService = scope.ServiceProvider.GetRequiredService<ICustomerService>();
    }

    public void ClearDatabase()
    {
        EnsureTestDatabase();

        using var context = CreateDbContext();

        var tables = context.Model.GetEntityTypes()
            .Where(t => t.ClrType.GetCustomAttributes(typeof(KeylessAttribute), true).Length == 0)
            .Select(t => t.GetTableName())
            .Where(t => t != null)
            .Distinct()
            .ToList();

        foreach (var table in tables)
            ClearTable(context, table!);
    }

    private void EnsureTestDatabase()
    {
        using var ctx = CreateDbContext();
        var dbName = ctx.Database.GetDbConnection().Database;

        if (!string.Equals(dbName, RequiredTestDbName, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"⛔ SICHERHEIT: Testoperationen sind nur auf '{RequiredTestDbName}' erlaubt.\n" +
                $"   Aktuelle Datenbank: '{dbName}'\n" +
                $"   Bitte ConnectionString in appsettings.json prüfen.");
        }
    }

    private static void ClearTable(AppDbContext context, string tableName)
    {
        var fs = FormattableStringFactory.Create($"DELETE FROM {tableName}");
        context.Database.ExecuteSql(fs);
    }

    private static IConfiguration ObtainConfiguration() =>
        new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

    // Passe dies an deinen echten Host-Builder an
    private static IHost CreateTestHostInstance()
    {
        var builder = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(cfg => cfg.AddJsonFile("appsettings.json"))
            .ConfigureServices((ctx, services) =>
            {
                // Deine DI-Registrierungen hier – z.B.:
                // services.AddTuloInfrastructure(ctx.Configuration);
                // services.AddTuloApplication();
            });

        return builder.Build();
    }
}
