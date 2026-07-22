using Bogus;
using Tulo.Domain.Entitites;

namespace TestList;
// InvoiceId als Parameter, da FK zu InvoiceHeader
public sealed class InvoicePositionFaker : Faker<InvoicePosition>
{
    public InvoicePositionFaker(Guid invoiceId)
    {
        RuleFor(p => p.Id, _ => Guid.NewGuid());
        RuleFor(p => p.InvoiceId, _ => invoiceId);
        RuleFor(p => p.ProductId, _ => null);
        RuleFor(p => p.Description, f => f.Commerce.ProductName());
        RuleFor(p => p.Quantity, f => Math.Round(f.Random.Decimal(1, 100), 2));
        RuleFor(p => p.UnitCode, _ => "C62");
        RuleFor(p => p.UnitPriceNet, f => Math.Round(f.Random.Decimal(1, 500), 2));
        RuleFor(p => p.TaxPercent, f => f.PickRandom(7m, 19m));
        RuleFor(p => p.TaxCategory, _ => "S");
    }
}

