using Bogus;
using Tulo.Domain.Entitites;

namespace TestList;

public sealed class CustomerFaker : Faker<Customer>
{
    public CustomerFaker()
    {
        RuleFor(c => c.Id, f => Guid.NewGuid());
        RuleFor(c => c.Name, f => f.Company.CompanyName());
        RuleFor(c => c.Street, f => f.Address.StreetAddress());
        RuleFor(c => c.City, f => f.Address.City());
        RuleFor(c => c.Zip, f => f.Address.ZipCode("#####"));
        RuleFor(c => c.CountryCode, f => "DE");
        RuleFor(c => c.ContactEmail, f => f.Internet.Email());
        RuleFor(c => c.ContactPhone, f => f.Phone.PhoneNumber());
        RuleFor(c => c.VatId, f => $"DE{f.Random.Number(100000000, 999999999)}");
    }
}
