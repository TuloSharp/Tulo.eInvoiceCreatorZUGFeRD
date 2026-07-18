using Tulo.Application.Enums;
using Tulo.Domain.Entitites;

namespace Tulo.Application.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken ct = default);
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Customer>> GetFilteredSortedAsync(string? search, CustomerSortBy sortBy, CancellationToken ct = default);
    Task AddAsync(Customer customer, CancellationToken ct = default);
    void Update(Customer customer);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
