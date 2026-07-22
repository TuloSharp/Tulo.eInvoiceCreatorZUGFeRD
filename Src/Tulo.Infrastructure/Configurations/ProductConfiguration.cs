using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tulo.Domain.Entitites;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> entity)
    {
        entity.HasKey(p => p.Id);
        entity.Property(p => p.Id).ValueGeneratedNever();

        entity.Property(p => p.Description).IsRequired().HasMaxLength(500);
        entity.Property(p => p.ProductDescription).HasMaxLength(1000);
        entity.Property(p => p.SellerAssignedId).HasMaxLength(50);
        entity.Property(p => p.GlobalId).HasMaxLength(100);
        entity.Property(p => p.GlobalIdSchemeId).HasMaxLength(50);
        entity.Property(p => p.OriginCountryCode).HasMaxLength(2);
        entity.Property(p => p.UnitCode).HasMaxLength(10);
        entity.Property(p => p.TaxCategory).HasMaxLength(5);

        entity.Property(p => p.UnitPriceNet).HasPrecision(18, 4);
        entity.Property(p => p.TaxPercent).HasPrecision(5, 2);
        entity.Property(p => p.PriceBasisQuantity).HasPrecision(18, 4);

        entity.HasIndex(p => p.SellerAssignedId);
        entity.HasIndex(p => p.Description);
    }
}

