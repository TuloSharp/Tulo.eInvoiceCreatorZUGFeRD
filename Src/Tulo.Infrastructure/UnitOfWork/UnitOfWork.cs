using Microsoft.EntityFrameworkCore;
using Tulo.Application.Interfaces.Repositories;
using Tulo.Application.Interfaces.UnitOfWorks;
using Tulo.Infrastructure.Repositories;

namespace Tulo.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext = null!;
    private bool _disposed;

    public ICustomerRepository CustomerRepository { get; }
    public ISellerRepository SellerRepository { get; }


    public UnitOfWork(IDbContextFactory<AppDbContext> dbFactory)
    {
        _dbContext = dbFactory.CreateDbContext();
   

        CustomerRepository = new CustomerRepository(_dbContext);
        SellerRepository = new SellerRepository(_dbContext);
    }

    public Task<int> CompleteAsync(CancellationToken ct = default)
    {
        ThrowIfDisposed();
        return _dbContext.SaveChangesAsync(ct);
    }
    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        _disposed = true;
        await _dbContext.DisposeAsync().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(_disposed, this);
}
