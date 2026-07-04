using PdfSharp.Pdf;
using Tulo.UpgradeToPdfA3.Interfaces;
using Tulo.UpgradeToPdfA3.ResultPattern;

namespace Tulo.UpgradeToPdfA3.Services;

public sealed class PdfALanguageWriter : IPdfALanguageWriter
{
    public OperationResult Write(PdfDocument pdfDocument, string? language)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(language))
                pdfDocument.Internals.Catalog.Elements["/Lang"] = new PdfString(language);

            return OperationResult.Ok();
        }
        catch (Exception ex)
        {
            return OperationResult.Fail($"Failed to set PDF language: {ex.Message}");
        }
    }
}
