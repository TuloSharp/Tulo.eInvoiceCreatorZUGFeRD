using tulo.CoreLib.Components.ResultPattern;
using Tulo.Application.Enums;
using Tulo.Application.ResultPattern;
using Tulo.Domain.Entitites;

namespace Tulo.Application.Interfaces.Services;
public interface IProductService
{
    List<Product> Products { get; }
    PagedResult<Product> CurrentPage { get; }

    event Action<Product>? ProductAdded;
    event Action<Product>? ProductUpdated;
    event Action<Guid>? ProductDeleted;
    event Action? ProductsLoaded;

    Task<OperationResult> AddAsync(Product product);
    Task<OperationResult> UpdateAsync(Product product);
    Task<OperationResult> DeleteAsync(Guid id);
    Task LoadAllAsync();
    Task LoadPagedAsync(string? search, ProductSortBy sortBy, int page, int pageSize);
}
