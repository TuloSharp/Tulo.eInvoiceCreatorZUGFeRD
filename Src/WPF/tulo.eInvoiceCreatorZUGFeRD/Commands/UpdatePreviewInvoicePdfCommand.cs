using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using tulo.CommonMVVM.Collector;
using tulo.CommonMVVM.Commands;
using tulo.CoreLib.PDFs;
using tulo.CoreLib.Translators;
using Tulo.eInvoiceCreatorZUGFeRD.Options;
using Tulo.eInvoiceCreatorZUGFeRD.Services;
using Tulo.eInvoiceCreatorZUGFeRD.ViewModels.Invoices;
using Tulo.eInvoiceXmlGeneratorCii.Mappers;
using Tulo.eInvoiceXmlGeneratorCii.Services;
using Tulo.XMLeInvoiceToPdf.Services;

namespace Tulo.eInvoiceCreatorZUGFeRD.Commands;

public class UpdatePreviewInvoicePdfCommand(InvoiceViewModel invoiceViewModel, ICollectorCollection collectorCollection) : BaseCommand
{
    private readonly ICollectorCollection _collectorCollection = collectorCollection;
    private readonly InvoiceViewModel _invoiceViewModel = invoiceViewModel;

    private readonly IInvoiceBuilderService _invoiceBuilderService = collectorCollection.GetService<IInvoiceBuilderService>();
    private readonly ICiiMapper _ciiMapper = collectorCollection.GetService<ICiiMapper>();
    private readonly IXmlCiiExporter _xmlCiiExporter = collectorCollection.GetService<IXmlCiiExporter>();
    private readonly IPdfGeneratorFromInvoice _pdfGeneratorFromInvoice = collectorCollection.GetService<IPdfGeneratorFromInvoice>();
    private readonly IPdfWatermarkService _watermarkService = collectorCollection.GetService<IPdfWatermarkService>();
    private readonly IOptions<AppOptions> _appOptions = collectorCollection.GetService<IOptions<AppOptions>>();
    private readonly ITranslatorUiProvider _translatorUiProvider = collectorCollection.GetService<ITranslatorUiProvider>();
    private readonly ILogger<UpdatePreviewInvoicePdfCommand> _logger = collectorCollection.GetService<ILoggerFactory>().CreateLogger<UpdatePreviewInvoicePdfCommand>();

    public override void Execute(object parameter)
    {
        try
        {
            _logger.LogInformation("[{Cmd}] Preview refresh started.", nameof(UpdatePreviewInvoicePdfCommand));

            _invoiceViewModel.IsPreviewEnabled = true;

            var invoice = Task.Run(async () =>
            {
                return await _invoiceBuilderService.BuildAsync(_invoiceViewModel, default);
            }).GetAwaiter().GetResult();

            var cii = _ciiMapper.Map(invoice);
            string xmlInvoiceContent = _xmlCiiExporter.ToXml(cii);

            var companyLogopath = _appOptions?.Value?.CompanyLogo?.LogoPath ?? string.Empty;

            using var pdfStream = _pdfGeneratorFromInvoice.GeneratePdfStream(
                xmlInvoiceFileName: string.Empty,
                xmlInvoiceContent: xmlInvoiceContent,
                false,
                companyLogopath);

            if (pdfStream is null)
            {
                _logger.LogWarning("[{Cmd}] PDF generation returned null.", nameof(UpdatePreviewInvoicePdfCommand));
                _invoiceViewModel.DocumentSource = $"<html><body><h1>{_translatorUiProvider.Translate("CreateInvoiceCmd_ErrorPdfNull")}</h1></body></html>";
                return;
            }

            var pdfMemoryStream = new MemoryStream();
            pdfStream.Position = 0;
            pdfStream.CopyTo(pdfMemoryStream);

            if (pdfMemoryStream.Length == 0)
            {
                _logger.LogWarning("[{Cmd}] PDF stream is empty.", nameof(UpdatePreviewInvoicePdfCommand));
                pdfMemoryStream.Dispose();
                _invoiceViewModel.DocumentSource = $"<html><body><h1>{_translatorUiProvider.Translate("CreateInvoiceCmd_ErrorPdfEmpty")}</h1></body></html>";
                return;
            }

            pdfMemoryStream.Position = 0;

            var watermarked = _watermarkService.AddWatermark(pdfMemoryStream, "PREVIEW");
            watermarked.Position = 0;

            _invoiceViewModel.DocumentSource = HtmlPdfRenderer.CreateHtmlViewerFromPdf(watermarked);

            if (!ReferenceEquals(watermarked, pdfMemoryStream))
                pdfMemoryStream.Dispose();

            _logger.LogInformation("[{Cmd}] Preview refresh completed successfully.", nameof(UpdatePreviewInvoicePdfCommand));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[{Cmd}] Preview refresh failed.", nameof(UpdatePreviewInvoicePdfCommand));

            string errorMessage = System.Net.WebUtility.HtmlEncode(ex.Message);
            _invoiceViewModel.DocumentSource = $"<html><body><h1>The selected file is not an eInvoice: {errorMessage}</h1></body></html>";
        }
    }
}

