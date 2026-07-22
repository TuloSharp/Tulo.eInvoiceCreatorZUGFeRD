using Bogus;
using Tulo.Domain.Entitites;

namespace TestList;
public sealed class ProductFaker : Faker<Product>
{
    public ProductFaker()
    {
        RuleFor(p => p.Id, _ => Guid.NewGuid());
        RuleFor(p => p.Description, f => f.Commerce.ProductName());
        RuleFor(p => p.ProductDescription, f => f.Commerce.ProductDescription());
        RuleFor(p => p.SellerAssignedId, f => f.Commerce.Ean8());
        RuleFor(p => p.GlobalId, f => f.Commerce.Ean13());
        RuleFor(p => p.GlobalIdSchemeId, _ => "0160");
        RuleFor(p => p.OriginCountryCode, _ => "DE");
        RuleFor(p => p.UnitCode, _ => "C62");
        RuleFor(p => p.UnitPriceNet, f => Math.Round(f.Random.Decimal(1, 500), 2));
        RuleFor(p => p.PriceBasisQuantity, _ => null);
        RuleFor(p => p.TaxPercent, f => f.PickRandom(7m, 19m));
        RuleFor(p => p.TaxCategory, _ => "S");
    }
}

