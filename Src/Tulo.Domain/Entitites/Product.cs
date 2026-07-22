namespace Tulo.Domain.Entitites;
public class Product
{
    public Guid Id { get; set; }

    // Product name and detailed description
    public string Description { get; set; } = string.Empty;
    public string ProductDescription { get; set; } = string.Empty;

    // Product identifiers
    public string SellerAssignedId { get; set; } = string.Empty;
    public string GlobalId { get; set; } = string.Empty;
    public string GlobalIdSchemeId { get; set; } = string.Empty;

    // Country of origin
    public string OriginCountryCode { get; set; } = string.Empty;

    // Unit and default net price
    public string UnitCode { get; set; } = "C62";
    public decimal UnitPriceNet { get; set; }
    public decimal? PriceBasisQuantity { get; set; }

    // Default tax settings
    public decimal TaxPercent { get; set; }
    public string TaxCategory { get; set; } = "S";
}
