using Microsoft.EntityFrameworkCore;
using Tulo.Application.Interfaces.Repositories;
using Tulo.Application.ResultPattern;
using Tulo.Domain.Entitites;

namespace Tulo.Infrastructure.Repositories;
public sealed class InvoicePositionRepository(AppDbContext dbContext) : IInvoicePositionRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<IReadOnlyList<InvoicePosition>> GetAllByInvoiceIdAsync(Guid invoiceId, CancellationToken ct = default)
    {
        return await _dbContext.InvoicePositions.AsNoTracking().Where(p => p.InvoiceId == invoiceId).OrderBy(p => p.SortOrder).ToListAsync(ct);
    }

    public async Task<InvoicePosition?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbContext.InvoicePositions.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<PagedResult<InvoicePosition>> GetPagedByInvoiceIdAsync(Guid invoiceId, int page, int pageSize, CancellationToken ct = default)
    {
        IQueryable<InvoicePosition> query = _dbContext.InvoicePositions.AsNoTracking().Where(p => p.InvoiceId == invoiceId).OrderBy(p => p.SortOrder);

        int totalCount = await query.CountAsync(ct);

        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new PagedResult<InvoicePosition>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task AddAsync(InvoicePosition invoicePosition, CancellationToken ct = default)
    {
        await _dbContext.InvoicePositions.AddAsync(invoicePosition, ct);
    }

    public void Update(InvoicePosition invoicePosition) => _dbContext.InvoicePositions.Update(invoicePosition);
  

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        await _dbContext.InvoicePositions.Where(p => p.Id == id).ExecuteDeleteAsync(ct);
    }

    public async Task DeleteAllByInvoiceIdAsync(Guid invoiceId, CancellationToken ct = default)
    {
        await _dbContext.InvoicePositions.Where(p => p.InvoiceId == invoiceId).ExecuteDeleteAsync(ct);
    }

    public void UpdateRange(IEnumerable<InvoicePosition> positions) => _dbContext.InvoicePositions.UpdateRange(positions);

    public async Task DeleteWithChildrenAsync(Guid id, CancellationToken ct = default)
    {
        await _dbContext.InvoicePositions.Where(p => p.ParentPositionId == id).ExecuteDeleteAsync(ct);

        await _dbContext.InvoicePositions.Where(p => p.Id == id).ExecuteDeleteAsync(ct);
    }

    public async Task<int> GetNextSortOrderAsync(Guid invoiceId, CancellationToken ct = default)
    {
        var max = await _dbContext.InvoicePositions.Where(p => p.InvoiceId == invoiceId).MaxAsync(p => (int?)p.SortOrder, ct);

        return (max ?? 0) + 1;
    }
}
