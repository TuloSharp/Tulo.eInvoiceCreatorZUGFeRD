using Bogus;
using Tulo.Domain.Entitites;

namespace TestList;

public sealed class SellerFaker : Faker<Seller>
{
    public SellerFaker()
    {
        RuleFor(s => s.Id, f => Guid.NewGuid());
        RuleFor(s => s.Name, f => f.Company.CompanyName());
        RuleFor(s => s.Street, f => f.Address.StreetAddress());
        RuleFor(s => s.City, f => f.Address.City());
        RuleFor(s => s.Zip, f => f.Address.ZipCode("#####"));
        RuleFor(s => s.CountryCode, f => "DE");
        RuleFor(s => s.ContactEmail, f => f.Internet.Email());
        RuleFor(s => s.ContactPhone, f => f.Phone.PhoneNumber());
        RuleFor(s => s.VatId, f => $"DE{f.Random.Number(100000000, 999999999)}");
    }
}
