using Microsoft.EntityFrameworkCore;
using Tulo.Application.Enums;
using Tulo.Application.Interfaces.Repositories;
using Tulo.Application.ResultPattern;
using Tulo.Domain.Entitites;

namespace Tulo.Infrastructure.Repositories;
public sealed class InvoiceHeaderRepository(AppDbContext dbContext) : IInvoiceHeaderRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<IReadOnlyList<InvoiceHeader>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbContext.InvoiceHeaders.AsNoTracking().OrderByDescending(i => i.InvoiceDate).ToListAsync(ct);
    }

    public async Task<InvoiceHeader?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbContext.InvoiceHeaders.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id, ct);
    }

    public async Task<InvoiceHeader?> GetByInvoiceNumberAsync(string invoiceNumber, CancellationToken ct = default)
    {
        return await _dbContext.InvoiceHeaders.AsNoTracking().FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber, ct);
    }

    public async Task<PagedResult<InvoiceHeader>> GetPagedAsync(string? search, InvoiceHeaderSortBy sortBy, int page, int pageSize, DateOnly? dateFrom, DateOnly? dateTo, CancellationToken ct = default)
    {
        IQueryable<InvoiceHeader> query = _dbContext.InvoiceHeaders.AsNoTracking();

        // ---- FILTER ----
        if (!string.IsNullOrWhiteSpace(search))
        {
            string normalized = search.ToLower();
            query = query.Where(i =>
                i.InvoiceNumber.ToLower().Contains(normalized) ||
                (i.FileName != null && i.FileName.ToLower().Contains(normalized)));
        }

        // ---- DATE RANGE ----
        if (dateFrom.HasValue)
            query = query.Where(i => i.InvoiceDate >= dateFrom.Value);

        if (dateTo.HasValue)
            query = query.Where(i => i.InvoiceDate <= dateTo.Value);

        // ---- SORT ----
        query = sortBy switch
        {
            InvoiceHeaderSortBy.InvoiceNumber => query.OrderBy(i => i.InvoiceNumber),
            InvoiceHeaderSortBy.InvoiceDate => query.OrderByDescending(i => i.InvoiceDate),
            InvoiceHeaderSortBy.FileName => query.OrderBy(i => i.FileName),
            _ => query.OrderByDescending(i => i.InvoiceDate)
        };

        int totalCount = await query.CountAsync(ct);

        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new PagedResult<InvoiceHeader>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task AddAsync(InvoiceHeader invoiceHeader, CancellationToken ct = default)
    {
        await _dbContext.InvoiceHeaders.AddAsync(invoiceHeader, ct);
    }

    public void Update(InvoiceHeader invoiceHeader) => _dbContext.InvoiceHeaders.Update(invoiceHeader);

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        await _dbContext.InvoiceHeaders.Where(i => i.Id == id).ExecuteDeleteAsync(ct);
    }
}
