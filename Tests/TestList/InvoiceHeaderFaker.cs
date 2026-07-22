using Bogus;
using Tulo.Domain.Entitites;

namespace TestList;
// CustomerId & SellerId als Parameter, da FK Constraints auf SQL Server aktiv sind
public sealed class InvoiceHeaderFaker : Faker<InvoiceHeader>
{
    public InvoiceHeaderFaker(Guid customerId, Guid sellerId)
    {
        RuleFor(i => i.Id, _ => Guid.NewGuid());
        RuleFor(i => i.InvoiceNumber, f => $"RE-{f.Random.Number(1000, 9999)}-{f.Random.Number(10, 99)}");
        RuleFor(i => i.InvoiceDate, f => DateOnly.FromDateTime(f.Date.Recent(365)));
        RuleFor(i => i.CustomerId, _ => customerId);
        RuleFor(i => i.SellerId, _ => sellerId);
        RuleFor(i => i.FileName, _ => null);
    }
}

