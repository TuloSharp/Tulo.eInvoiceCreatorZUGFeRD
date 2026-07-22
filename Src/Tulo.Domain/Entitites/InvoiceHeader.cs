namespace Tulo.Domain.Entitites;
public class InvoiceHeader
{
    public Guid Id { get; set; }

    // Invoice number (display / document)
    public string InvoiceNumber { get; set; } = string.Empty;

    // Invoice date
    public DateOnly InvoiceDate { get; set; }

    // Reference to the buyer (Customer)
    public Guid CustomerId { get; set; }

    // Reference to the seller
    public Guid SellerId { get; set; }

    // Generated ZUGFeRD file
    public string? FileName { get; set; }
}
