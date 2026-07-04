using Zugferd24.Extended;

namespace Tulo.eInvoiceXmlGeneratorCii.Services;

public interface IXmlCiiExporter
{
    string ToXml(CrossIndustryInvoiceType invoice);
}
