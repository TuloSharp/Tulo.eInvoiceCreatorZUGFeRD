using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tulo.Domain.Entitites;

namespace Tulo.Infrastructure.Configurations;
public sealed class SellerConfiguration : IEntityTypeConfiguration<Seller>
{
    public void Configure(EntityTypeBuilder<Seller> entity)
    {
        entity.HasKey(s => s.Id);
        entity.Property(s => s.Id).ValueGeneratedNever();

        entity.Property(s => s.Name).IsRequired().HasMaxLength(200);
        entity.Property(s => s.Street).HasMaxLength(200);
        entity.Property(s => s.Zip).HasMaxLength(20);
        entity.Property(s => s.City).HasMaxLength(100);
        entity.Property(s => s.CountryCode).HasMaxLength(2);
        entity.Property(s => s.PartyId).HasMaxLength(50);
        entity.Property(s => s.IdSchemeId).HasMaxLength(50);
        entity.Property(s => s.VatId).HasMaxLength(50);
        entity.Property(s => s.TaxRegistrationFcId).HasMaxLength(50);
        entity.Property(s => s.FiscalId).HasMaxLength(50);
        entity.Property(s => s.LegalOrganizationId).HasMaxLength(100);
        entity.Property(s => s.LeitwegId).HasMaxLength(100);
        entity.Property(s => s.LeitwegIdSchemeId).HasMaxLength(50);
        entity.Property(s => s.GlobalId).HasMaxLength(100);
        entity.Property(s => s.GlobalIdSchemeId).HasMaxLength(50);
        entity.Property(s => s.GeneralEmail).HasMaxLength(200);
        entity.Property(s => s.ContactPersonName).HasMaxLength(200);
        entity.Property(s => s.ContactPhone).HasMaxLength(50);
        entity.Property(s => s.ContactEmail).HasMaxLength(200);

        entity.HasIndex(s => s.Name);
        entity.HasIndex(s => s.VatId);
    }
}

