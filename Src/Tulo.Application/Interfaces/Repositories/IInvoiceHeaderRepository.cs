using Tulo.Application.Enums;
using Tulo.Application.ResultPattern;
using Tulo.Domain.Entitites;

namespace Tulo.Application.Interfaces.Repositories;
public interface IInvoiceHeaderRepository
{
    Task<IReadOnlyList<InvoiceHeader>> GetAllAsync(CancellationToken ct = default);
    Task<InvoiceHeader?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<InvoiceHeader?> GetByInvoiceNumberAsync(string invoiceNumber, CancellationToken ct = default);
    Task<PagedResult<InvoiceHeader>> GetPagedAsync(string? search, InvoiceHeaderSortBy sortBy, int page, int pageSize, DateOnly? dateFrom, DateOnly? dateTo, CancellationToken ct = default);
    Task AddAsync(InvoiceHeader invoiceHeader, CancellationToken ct = default);
    void Update(InvoiceHeader invoiceHeader);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}

