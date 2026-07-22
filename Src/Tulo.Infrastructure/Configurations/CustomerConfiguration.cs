using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tulo.Domain.Entitites;

namespace Tulo.Infrastructure.Configurations;
public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> entity)
    {
        entity.HasKey(c => c.Id);
        entity.Property(c => c.Id).ValueGeneratedNever();

        entity.Property(c => c.Name).IsRequired().HasMaxLength(200);
        entity.Property(c => c.Street).HasMaxLength(200);
        entity.Property(c => c.Zip).HasMaxLength(20);
        entity.Property(c => c.City).HasMaxLength(100);
        entity.Property(c => c.CountryCode).HasMaxLength(2);
        entity.Property(c => c.PartyId).HasMaxLength(50);
        entity.Property(c => c.IdSchemeId).HasMaxLength(50);
        entity.Property(c => c.VatId).HasMaxLength(50);
        entity.Property(c => c.TaxRegistrationFcId).HasMaxLength(50);
        entity.Property(c => c.FiscalId).HasMaxLength(50);
        entity.Property(c => c.LegalOrganizationId).HasMaxLength(100);
        entity.Property(c => c.LeitwegId).HasMaxLength(100);
        entity.Property(c => c.LeitwegIdSchemeId).HasMaxLength(50);
        entity.Property(c => c.GlobalId).HasMaxLength(100);
        entity.Property(c => c.GlobalIdSchemeId).HasMaxLength(50);
        entity.Property(c => c.GeneralEmail).HasMaxLength(200);
        entity.Property(c => c.ContactPersonName).HasMaxLength(200);
        entity.Property(c => c.ContactPhone).HasMaxLength(50);
        entity.Property(c => c.ContactEmail).HasMaxLength(200);

        entity.HasIndex(c => c.Name);
        entity.HasIndex(c => c.VatId);
        entity.HasIndex(c => c.PartyId);
    }
}

