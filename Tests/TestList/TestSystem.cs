using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PdfSharp.Fonts;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Tulo.Application.Interfaces.Services;
using Tulo.Application.Interfaces.UnitOfWorks;
using Tulo.eInvoiceCreatorZUGFeRD;
using Tulo.eInvoiceCreatorZUGFeRD.Options;
using Tulo.Infrastructure;
using Tulo.XMLeInvoiceToPdf.Utilities;

namespace TestList;

public class TestSystem
{
    public AppDbContext CreateDbContext() => new(_dbContextOptions);
    public IUnitOfWorkFactory UnitOfWorkFactory = null!;
    public ISellerService SellerService = null!;
    public ICustomerService CustomerService = null!;
    private const string RequiredTestDbName = "InvoiceManagerDb-Tests";
    public IAppOptions AppOptions = null!;

    private readonly string _appSettingsKeyConnectionString = "connection:connectionstring";
    private readonly DbContextOptions<AppDbContext> _dbContextOptions = null!;

    public TestSystem()
    {
        var config = ObtainConfiguration();
        var connectionString = config.GetValue<string>(_appSettingsKeyConnectionString) ?? throw new InvalidOperationException("connection string not configured.");

        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(connectionString).Options;

        EnsureTestDatabase();
        GlobalFontSettings.FontResolver = new EmbeddedFontResolver();
    }

    public void ClearTestSystem()
    {
        Trace.TraceInformation(nameof(ClearTestSystem));
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
        AppOptions = scope.ServiceProvider.GetRequiredService<IAppOptions>();
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

    private static IHost CreateTestHostInstance()
    {
        var app = new UiApplication(null!);
        var hostBuilder = app.InitializeHostBuilder();
        return app.BuildHost(hostBuilder);
    }
}
