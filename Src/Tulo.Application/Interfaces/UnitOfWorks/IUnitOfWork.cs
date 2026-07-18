using Tulo.Application.Interfaces.Repositories;

namespace Tulo.Application.Interfaces.UnitOfWorks;

public interface IUnitOfWork : IAsyncDisposable
{
    ICustomerRepository CustomerRepository { get; }
    ISellerRepository SellerRepository { get; }


    Task<int> CompleteAsync(CancellationToken ct = default);
}
