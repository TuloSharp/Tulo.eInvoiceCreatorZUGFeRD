using Microsoft.Extensions.Logging;
using tulo.CommonMVVM.Collector;
using tulo.CommonMVVM.Commands;
using Tulo.eInvoiceCreatorZUGFeRD.Services;
using Tulo.eInvoiceCreatorZUGFeRD.ViewModels.Invoices;

namespace Tulo.eInvoiceCreatorZUGFeRD.Commands.Invoices;
public class DeleteInvoicePositionDetailsCommand(DeleteInvoicePositionViewModel deleteInvoicePositionViewModel, ICollectorCollection collectorCollection) : AsyncBaseCommand
{
    private DeleteInvoicePositionViewModel _deleteInvoicePositionViewModel = deleteInvoicePositionViewModel;

    #region Services / Stores filled via CollectorCollection
    private readonly ILogger<DeleteInvoicePositionDetailsCommand> _logger = collectorCollection.GetService<ILoggerFactory>().CreateLogger<DeleteInvoicePositionDetailsCommand>();
    private readonly IInvoicePositionServiceByStore _invoicePositionService = collectorCollection.GetService<IInvoicePositionServiceByStore>();
    #endregion

    protected override async Task ExecuteAsync(object parameter)
    {
        _logger.LogInformation($"{nameof(DeleteInvoicePositionDetailsCommand)} start exection");

        _deleteInvoicePositionViewModel.StatusMessage = string.Empty;

        InvoicePositionDetailsFormViewModel invPosDetailsViewModel = _deleteInvoicePositionViewModel.InvoicePositionDetailsFormViewModel;

        try
        {
            await _invoicePositionService.DeleteInvoicePositionAsync(invPosDetailsViewModel.Id);
            if (_invoicePositionService.IsDeleted)
            {
                _deleteInvoicePositionViewModel.StatusMessage = string.Empty;

                //deactivate confirmUnsavedChangesOnClose
                var @params = new object[] { null!, false };
                _deleteInvoicePositionViewModel.CloseDeleteInvoicePositionDetailsCommand.Execute(parameter: @params);
                _logger.LogInformation($"{nameof(DeleteInvoicePositionDetailsCommand)} is closed");
            }
            else
            {
                _deleteInvoicePositionViewModel.InvalidStateAtInputField = _invoicePositionService.AreRequiredFieldsFilled;
                _deleteInvoicePositionViewModel.StatusMessage = _invoicePositionService.StatusMessage;
                _logger.LogInformation(_invoicePositionService.StatusMessage);
            }
        }
        catch (Exception e)
        {
            _deleteInvoicePositionViewModel.StatusMessage = "Technical error" + _invoicePositionService.StatusMessage + $"\n{e.Message}";
            _logger.LogError(e, _invoicePositionService.StatusMessage);
        }
        finally
        {
            _logger.LogInformation(invPosDetailsViewModel.InvoicePositionNr.ToString() + " is deleted = " + _invoicePositionService.IsDeleted);
            _logger.LogInformation($"{nameof(DeleteInvoicePositionDetailsCommand)} was executed");
        }
    }
}
