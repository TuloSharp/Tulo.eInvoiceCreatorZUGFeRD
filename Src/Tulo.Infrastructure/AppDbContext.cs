using Microsoft.EntityFrameworkCore;
using Tulo.Domain.Entitites;

namespace Tulo.Infrastructure;

public sealed class AppDbContext : DbContext
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Seller> Sellers => Set<Seller>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(entity =>
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
        });

        modelBuilder.Entity<Seller>(entity =>
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
        });
    }

}
