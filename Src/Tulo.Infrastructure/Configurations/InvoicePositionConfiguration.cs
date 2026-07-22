using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tulo.Domain.Entitites;

namespace Tulo.Infrastructure.Configurations;
public sealed class InvoicePositionConfiguration : IEntityTypeConfiguration<InvoicePosition>
{
    public void Configure(EntityTypeBuilder<InvoicePosition> entity)
    {
        entity.HasKey(p => p.Id);
        entity.Property(p => p.Id).ValueGeneratedNever();

        entity.Property(p => p.InvoiceId).IsRequired();
        entity.Property(p => p.Description).IsRequired().HasMaxLength(500);
        entity.Property(p => p.UnitCode).HasMaxLength(10);
        entity.Property(p => p.TaxCategory).HasMaxLength(5);

        entity.Property(p => p.Quantity).HasPrecision(18, 4);
        entity.Property(p => p.UnitPriceNet).HasPrecision(18, 4);
        entity.Property(p => p.TaxPercent).HasPrecision(5, 2);

        entity.HasIndex(p => p.InvoiceId);
        entity.HasIndex(p => p.ProductId);
    }
}

