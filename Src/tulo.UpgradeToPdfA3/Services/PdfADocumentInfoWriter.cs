using PdfSharp.Pdf;
using Tulo.UpgradeToPdfA3.Options;
using Tulo.UpgradeToPdfA3.Interfaces;
using Tulo.UpgradeToPdfA3.ResultPattern;

namespace Tulo.UpgradeToPdfA3.Services;

public sealed class PdfADocumentInfoWriter : IPdfADocumentInfoWriter
{
    public OperationResult Write(PdfDocument pdfDocument, IUpgradeToPdfA3Options appOptions)
    {
        try
        {
            DateTime now = DateTime.UtcNow;

            pdfDocument.Info.Title = appOptions.PdfA3.Title ?? string.Empty;
            pdfDocument.Info.Subject = appOptions.PdfA3.Description ?? string.Empty;
            pdfDocument.Info.Author = appOptions.PdfA3.Author ?? string.Empty;
            pdfDocument.Info.Creator = appOptions.PdfA3.Creator ?? string.Empty;
            pdfDocument.Info.CreationDate = now;
            pdfDocument.Info.ModificationDate = now;

            return OperationResult.Ok();
        }
        catch (Exception ex)
        {
            return OperationResult.Fail($"Failed to apply PDF info: {ex.Message}");
        }
    }
}
