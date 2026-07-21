using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Tulo.Infrastructure;

namespace Tulo.eInvoiceCreatorZUGFeRD.HostBuilders;

public static class AddDbContextHostBuilderExtension
{
    /// <summary>
    /// Adds DbContext with SQL Server configuration to the host.
    /// </summary>
    /// <param name="host">The <see cref="IHostApplicationBuilder"/> to configure.</param>
    /// <returns>The configured <see cref="IHostApplicationBuilder"/>.</returns>
    public static IHostBuilder AddDbContext(this IHostBuilder host)
    {
        host.ConfigureServices((context, services) =>
        {
            var bootstrapLogger = Log.ForContext(typeof(AddDbContextHostBuilderExtension));
            var configuration = context.Configuration;
            var connectionString = configuration.GetValue<string>("connection:connectionstring");

            if (string.IsNullOrEmpty(connectionString))
            {
                bootstrapLogger.Error("{Extension}: 'connection:connectionstring' is not defined", nameof(AddDbContextHostBuilderExtension));
                return;
            }

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromMilliseconds(350),
                        errorNumbersToAdd: new[] { 4060 }
                    );
                });

                options.ConfigureWarnings(warnings =>
                    warnings.Ignore(SqlServerEventId.DecimalTypeKeyWarning));
            });

            services.AddDbContextFactory<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromMilliseconds(350),
                        errorNumbersToAdd: new[] { 4060 }
                    );
                });

                options.ConfigureWarnings(warnings =>
                    warnings.Ignore(SqlServerEventId.DecimalTypeKeyWarning));
            });

            bootstrapLogger.Information("DbContext has been initialized successfully.");
        });

        return host;
    }
}
