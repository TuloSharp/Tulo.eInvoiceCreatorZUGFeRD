using tulo.CoreLib.Components.ResultPattern;
using Tulo.Application.Enums;
using Tulo.Application.Interfaces.Services;
using Tulo.Application.Interfaces.UnitOfWorks;
using Tulo.Application.ResultPattern;
using Tulo.Domain.Entitites;

namespace Tulo.Application.Services;
public sealed class InvoiceHeaderService(IUnitOfWorkFactory uowFactory) : IInvoiceHeaderService
{
    public List<InvoiceHeader> InvoiceHeaders { get; private set; } = [];
    public PagedResult<InvoiceHeader> CurrentPage { get; private set; } = new();

    public event Action<InvoiceHeader>? InvoiceHeaderAdded;
    public event Action<InvoiceHeader>? InvoiceHeaderUpdated;
    public event Action<Guid>? InvoiceHeaderDeleted;
    public event Action? InvoiceHeadersLoaded;

    public async Task<OperationResult> AddAsync(InvoiceHeader invoiceHeader)
    {
        if (invoiceHeader is null) return OperationResult.Fail("InvoiceHeader is null");

        await using var uow = uowFactory.Create();

        await uow.InvoiceHeaderRepository.AddAsync(invoiceHeader);
        await uow.CompleteAsync();

        InvoiceHeaders.Add(invoiceHeader);
        InvoiceHeaderAdded?.Invoke(invoiceHeader);
        return OperationResult.Ok("Invoice created successfully");
    }

    public async Task<OperationResult> UpdateAsync(InvoiceHeader invoiceHeader)
    {
        if (invoiceHeader is null) return OperationResult.Fail("InvoiceHeader is null");

        await using var uow = uowFactory.Create();

        var existing = await uow.InvoiceHeaderRepository.GetByIdAsync(invoiceHeader.Id);
        if (existing is null) return OperationResult.Fail("Invoice not found");

        uow.InvoiceHeaderRepository.Update(invoiceHeader);
        await uow.CompleteAsync();

        var index = InvoiceHeaders.FindIndex(i => i.Id == invoiceHeader.Id);
        if (index >= 0) InvoiceHeaders[index] = invoiceHeader;
        InvoiceHeaderUpdated?.Invoke(invoiceHeader);
        return OperationResult.Ok("Invoice updated successfully");
    }

    public async Task<OperationResult> DeleteAsync(Guid id)
    {
        await using var uow = uowFactory.Create();

        await uow.InvoiceHeaderRepository.DeleteAsync(id);
        await uow.CompleteAsync();

        InvoiceHeaders.RemoveAll(i => i.Id == id);
        InvoiceHeaderDeleted?.Invoke(id);
        return OperationResult.Ok("Invoice deleted successfully");
    }

    public async Task LoadAllAsync()
    {
        await using var uow = uowFactory.Create();
        InvoiceHeaders = (await uow.InvoiceHeaderRepository.GetAllAsync()).ToList();
        InvoiceHeadersLoaded?.Invoke();
    }

    public async Task LoadPagedAsync(string? search, InvoiceHeaderSortBy sortBy, int page, int pageSize, DateOnly? dateFrom, DateOnly? dateTo)
    {
        await using var uow = uowFactory.Create();
        CurrentPage = await uow.InvoiceHeaderRepository.GetPagedAsync(search, sortBy, page, pageSize, dateFrom, dateTo);
        InvoiceHeaders = CurrentPage.Items.ToList();
        InvoiceHeadersLoaded?.Invoke();
    }
}
