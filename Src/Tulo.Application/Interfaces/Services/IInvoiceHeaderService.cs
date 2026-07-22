using tulo.CoreLib.Components.ResultPattern;
using Tulo.Application.Enums;
using Tulo.Application.ResultPattern;
using Tulo.Domain.Entitites;

namespace Tulo.Application.Interfaces.Services;
public interface IInvoiceHeaderService
{
    List<InvoiceHeader> InvoiceHeaders { get; }
    PagedResult<InvoiceHeader> CurrentPage { get; }

    event Action<InvoiceHeader>? InvoiceHeaderAdded;
    event Action<InvoiceHeader>? InvoiceHeaderUpdated;
    event Action<Guid>? InvoiceHeaderDeleted;
    event Action? InvoiceHeadersLoaded;

    Task<OperationResult> AddAsync(InvoiceHeader invoiceHeader);
    Task<OperationResult> UpdateAsync(InvoiceHeader invoiceHeader);
    Task<OperationResult> DeleteAsync(Guid id);
    Task LoadAllAsync();
    Task LoadPagedAsync(string? search, InvoiceHeaderSortBy sortBy, int page, int pageSize, DateOnly? dateFrom, DateOnly? dateTo);
}
