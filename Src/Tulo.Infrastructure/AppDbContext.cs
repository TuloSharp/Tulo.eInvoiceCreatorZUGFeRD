using Microsoft.EntityFrameworkCore;
using Tulo.Domain.Entitites;

namespace Tulo.Infrastructure;

public sealed class AppDbContext : DbContext
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Seller> Sellers => Set<Seller>();
    public DbSet<InvoiceHeader> InvoiceHeaders => Set<InvoiceHeader>();
    public DbSet<InvoicePosition> InvoicePositions => Set<InvoicePosition>();
    public DbSet<Product> Products => Set<Product>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //instead of registering each entity configuration manually inside OnModelCreating,
        //EF Core scans the entire assembly at startup and automatically discovers every class that implements IEntityTypeConfiguration<T>,
        //calling its Configure() method. This means the AppDbContext never needs to be touched when a new entity
        //is added — simply create a new configuration class in the Configurations folder and it will be picked up automatically.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
