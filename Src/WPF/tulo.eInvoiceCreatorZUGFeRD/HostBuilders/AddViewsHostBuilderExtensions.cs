using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tulo.eInvoiceCreatorZUGFeRD.ViewModels;

namespace Tulo.eInvoiceCreatorZUGFeRD.HostBuilders;

public static class AddViewsHostBuilderExtensions
{
    public static IHostBuilder AddViews(this IHostBuilder host)
    {
        host.ConfigureServices(services =>
        {
            services.AddSingleton(s => new MainWindow(s.GetRequiredService<MainViewModel>()));
        });

        return host;
    }
}
