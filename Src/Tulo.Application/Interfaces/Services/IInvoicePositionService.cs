using tulo.CoreLib.Components.ResultPattern;
using Tulo.Application.DTOs;
using Tulo.Application.ResultPattern;
using Tulo.Domain.Entitites;

namespace Tulo.Application.Interfaces.Services;
public interface IInvoicePositionService
{
    List<InvoicePosition> Positions { get; }
    PagedResult<InvoicePosition> CurrentPage { get; }

    event Action<InvoicePosition>? PositionAdded;
    event Action<InvoicePosition>? PositionUpdated;
    event Action<Guid>? PositionDeleted;
    event Action? PositionsLoaded;

    Task<OperationResult> AddAsync(InvoicePosition position);
    Task<OperationResult> UpdateAsync(InvoicePosition position);
    Task<OperationResult> DeleteAsync(Guid id);
    Task<OperationResult> DeleteAllByInvoiceIdAsync(Guid invoiceId);
    Task LoadByInvoiceIdAsync(Guid invoiceId);
    Task LoadPagedByInvoiceIdAsync(Guid invoiceId, int page, int pageSize);

    int TotalCount { get; }
    Task<int> SuggestNextPositionNoAsync(Guid invoiceId);
    Task<int> SuggestNextSubPositionNoAsync(Guid invoiceId, Guid parentId);
    Task<OperationResult<Guid>> AddInvoicePositionAsync(Guid invoiceId, InvoicePositionDetailsDTO invPos, int? desiredPositionNo = null);
    Task<OperationResult<Guid>> AddSubInvoicePositionAsync(Guid invoiceId, Guid parentId, InvoicePositionDetailsDTO subPos);
    Task<OperationResult<Guid>> UpdateInvoicePositionAsync(Guid invoiceId, Guid id, InvoicePositionDetailsDTO invPos);
    Task<OperationResult<Guid>> DeleteInvoicePositionAsync(Guid invoiceId, Guid id);
    Task<OperationResult<List<InvoicePositionDetailsDTO>>> SetInvoicePositionNoAsync(Guid invoiceId, Guid id, int newPositionNo);
}
