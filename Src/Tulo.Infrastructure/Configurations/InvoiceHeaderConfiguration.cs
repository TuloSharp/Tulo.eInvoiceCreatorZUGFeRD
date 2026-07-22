using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tulo.Domain.Entitites;

namespace Tulo.Infrastructure.Configurations;
public sealed class InvoiceHeaderConfiguration : IEntityTypeConfiguration<InvoiceHeader>
{
    public void Configure(EntityTypeBuilder<InvoiceHeader> entity)
    {
        entity.HasKey(i => i.Id);
        entity.Property(i => i.Id).ValueGeneratedNever();

        entity.Property(i => i.InvoiceNumber).IsRequired().HasMaxLength(50);
        entity.Property(i => i.InvoiceDate).IsRequired();
        entity.Property(i => i.CustomerId).IsRequired();
        entity.Property(i => i.SellerId).IsRequired();
        entity.Property(i => i.FileName).HasMaxLength(500);

        entity.HasIndex(i => i.InvoiceNumber).IsUnique();
        entity.HasIndex(i => i.InvoiceDate);
        entity.HasIndex(i => i.CustomerId);
    }
}

