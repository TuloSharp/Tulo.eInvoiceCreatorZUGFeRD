using Microsoft.EntityFrameworkCore;
using Tulo.Application.Enums;
using Tulo.Application.Interfaces.Repositories;
using Tulo.Application.ResultPattern;
using Tulo.Domain.Entitites;

namespace Tulo.Infrastructure.Repositories;
public sealed class ProductRepository(AppDbContext dbContext) : IProductRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbContext.Products.AsNoTracking().OrderBy(p => p.Description).ToListAsync(ct);
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<PagedResult<Product>> GetPagedAsync(string? search, ProductSortBy sortBy, int page, int pageSize, CancellationToken ct = default)
    {
        IQueryable<Product> query = _dbContext.Products.AsNoTracking();

        // ---- FILTER ----
        if (!string.IsNullOrWhiteSpace(search))
        {
            string normalized = search.ToLower();
            query = query.Where(p =>
                p.Description.ToLower().Contains(normalized) ||
                p.SellerAssignedId.ToLower().Contains(normalized) ||
                p.ProductDescription.ToLower().Contains(normalized));
        }

        // ---- SORT ----
        query = sortBy switch
        {
            ProductSortBy.Description => query.OrderBy(p => p.Description),
            ProductSortBy.SellerAssignedId => query.OrderBy(p => p.SellerAssignedId),
            ProductSortBy.UnitPriceNet => query.OrderBy(p => p.UnitPriceNet),
            _ => query.OrderBy(p => p.Description)
        };

        int totalCount = await query.CountAsync(ct);

        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new PagedResult<Product>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task AddAsync(Product product, CancellationToken ct = default)
    {
        await _dbContext.Products.AddAsync(product, ct);
    }

    public void Update(Product product) => _dbContext.Products.Update(product);

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        await _dbContext.Products.Where(p => p.Id == id).ExecuteDeleteAsync(ct);
    }
}
