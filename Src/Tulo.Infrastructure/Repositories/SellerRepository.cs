using Microsoft.EntityFrameworkCore;
using Tulo.Application.Interfaces.Repositories;
using Tulo.Domain.Entitites;

namespace Tulo.Infrastructure.Repositories;

public sealed class SellerRepository : ISellerRepository
{
    private readonly AppDbContext _dbContext;

    public SellerRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Seller?> GetAsync(CancellationToken ct = default)
    {
        return await _dbContext.Sellers
            .AsNoTracking()
            .FirstOrDefaultAsync(ct);
    }

    public async Task AddAsync(Seller seller, CancellationToken ct = default)
    {
        await _dbContext.Sellers.AddAsync(seller, ct);
    }

    public void Update(Seller seller)
    {
        _dbContext.Sellers.Update(seller);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        await _dbContext.Sellers
            .Where(s => s.Id == id)
            .ExecuteDeleteAsync(ct);
    }
}
