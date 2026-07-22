namespace Tulo.Domain.Entitites;
public class InvoicePosition
{
    public Guid Id { get; set; }

    // Foreign key to Invoice
    public Guid InvoiceId { get; set; }

    // Reference to product catalog (optional)
    public Guid? ProductId { get; set; }

    //Who is my parent?
    public Guid? ParentPositionId { get; set; }
    // GROUP / DETAIL / empty
    public string LineStatusReasonCode { get; set; } = string.Empty;
    
    // Order
    public int SortOrder { get; set; }

    // Line description
    public string Description { get; set; } = string.Empty;

    // Quantity and unit
    public decimal Quantity { get; set; }
    public string UnitCode { get; set; } = "C62";

    // Net price and tax
    public decimal UnitPriceNet { get; set; }
    public decimal TaxPercent { get; set; }
    public string TaxCategory { get; set; } = "S";
}
