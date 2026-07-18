using Microsoft.EntityFrameworkCore;
using Tulo.Application.Enums;
using Tulo.Application.Interfaces.Repositories;
using Tulo.Domain.Entitites;

namespace Tulo.Infrastructure.Repositories;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _dbContext;

    public CustomerRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbContext.Customers
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync(ct);
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbContext.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task<IReadOnlyList<Customer>> GetFilteredSortedAsync(
        string? search, CustomerSortBy sortBy, CancellationToken ct = default)
    {
        IQueryable<Customer> query = _dbContext.Customers.AsNoTracking();

        // ---- FILTER ----
        if (!string.IsNullOrWhiteSpace(search))
        {
            string normalized = search.ToLower();
            query = query.Where(c =>
                c.Name.ToLower().Contains(normalized) ||
                c.City.ToLower().Contains(normalized) ||
                c.PartyId.ToLower().Contains(normalized));
        }

        // ---- SORT ----
        query = sortBy switch
        {
            CustomerSortBy.Name => query.OrderBy(c => c.Name),
            CustomerSortBy.City => query.OrderBy(c => c.City),
            CustomerSortBy.CountryCode => query.OrderBy(c => c.CountryCode),
            _ => query.OrderBy(c => c.Name)
        };

        return await query.ToListAsync(ct);
    }

    public async Task AddAsync(Customer customer, CancellationToken ct = default)
    {
        await _dbContext.Customers.AddAsync(customer, ct);
    }

    public void Update(Customer customer)
    {
        _dbContext.Customers.Update(customer);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        await _dbContext.Customers
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync(ct);
    }
}
