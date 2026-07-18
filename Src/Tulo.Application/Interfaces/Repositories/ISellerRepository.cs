using Tulo.Domain.Entitites;

namespace Tulo.Application.Interfaces.Repositories;

public interface ISellerRepository
{
    Task<Seller?> GetAsync(CancellationToken ct = default);
    Task AddAsync(Seller seller, CancellationToken ct = default);
    void Update(Seller seller);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
