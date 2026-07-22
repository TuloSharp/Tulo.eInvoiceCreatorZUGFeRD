using Tulo.Application.ResultPattern;
using Tulo.Domain.Entitites;

namespace Tulo.Application.Interfaces.Repositories;
public interface IInvoicePositionRepository
{
    Task<IReadOnlyList<InvoicePosition>> GetAllByInvoiceIdAsync(Guid invoiceId, CancellationToken ct = default);
    Task<InvoicePosition?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResult<InvoicePosition>> GetPagedByInvoiceIdAsync(Guid invoiceId, int page, int pageSize, CancellationToken ct = default);
    Task AddAsync(InvoicePosition invoicePosition, CancellationToken ct = default);
    void Update(InvoicePosition invoicePosition);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
    Task DeleteAllByInvoiceIdAsync(Guid invoiceId, CancellationToken ct = default);
    void UpdateRange(IEnumerable<InvoicePosition> positions);
    Task DeleteWithChildrenAsync(Guid id, CancellationToken ct = default);
    Task<int> GetNextSortOrderAsync(Guid invoiceId, CancellationToken ct = default);
}
