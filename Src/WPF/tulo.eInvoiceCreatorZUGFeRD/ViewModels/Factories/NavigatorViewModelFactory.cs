using Microsoft.Extensions.Logging;
using tulo.CommonMVVM.Collector;
using tulo.CommonMVVM.ViewModels;
using Tulo.eInvoiceCreatorZUGFeRD.Utilities;
using Tulo.eInvoiceCreatorZUGFeRD.ViewModels.About;
using Tulo.eInvoiceCreatorZUGFeRD.ViewModels.Invoices;
using Tulo.eInvoiceCreatorZUGFeRD.ViewModels.Sellers;

namespace Tulo.eInvoiceCreatorZUGFeRD.ViewModels.Factories
{
    public class NavigatorViewModelFactory(CreateViewModel<InvoiceViewModel> createInvoiceViewModel,
                                           CreateViewModel<SellerViewModel> createSellerViewModel,
                                           CreateViewModel<AboutViewModel> createAboutViewModel,
                                           ICollectorCollection collectorCollection) : INavigatorViewModelFactory
    {
        private readonly CreateViewModel<InvoiceViewModel> _createInvoiceViewModel = createInvoiceViewModel;
        private readonly CreateViewModel<SellerViewModel> _createSellerViewModel = createSellerViewModel;
        private readonly CreateViewModel<AboutViewModel> _createAboutViewModel = createAboutViewModel;
        private readonly ILogger<NavigatorViewModelFactory> _logger = collectorCollection.GetService<ILoggerFactory>().CreateLogger<NavigatorViewModelFactory>();

        public BaseViewModel CreateViewModel(NavTypes viewTypes)
        {
            switch (viewTypes)
            {
                case NavTypes.InvoiceView:
                    _logger.LogInformation($"{nameof(CreateViewModel)}: {nameof(NavTypes.InvoiceView)}");
                    return _createInvoiceViewModel();


                case NavTypes.SellerView:
                    _logger.LogInformation($"{nameof(CreateViewModel)}: {nameof(NavTypes.SellerView)}");
                    return _createSellerViewModel();

                case NavTypes.AboutView:
                    _logger.LogInformation($"{nameof(CreateViewModel)}: {nameof(NavTypes.AboutView)}");
                    return _createAboutViewModel();


                default:
                    var tempMessage = $"the ViewType doesn't have a ViewModel" + nameof(viewTypes) + "viewType";
                    _logger.LogError(tempMessage);
                    throw new ArgumentException(tempMessage);
            }
        }
    }
}
