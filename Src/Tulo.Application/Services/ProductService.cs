using tulo.CoreLib.Components.ResultPattern;
using Tulo.Application.Enums;
using Tulo.Application.Interfaces.Services;
using Tulo.Application.Interfaces.UnitOfWorks;
using Tulo.Application.ResultPattern;
using Tulo.Domain.Entitites;

namespace Tulo.Application.Services;
public sealed class ProductService(IUnitOfWorkFactory uowFactory) : IProductService
{
    public List<Product> Products { get; private set; } = [];
    public PagedResult<Product> CurrentPage { get; private set; } = new();

    public event Action<Product>? ProductAdded;
    public event Action<Product>? ProductUpdated;
    public event Action<Guid>? ProductDeleted;
    public event Action? ProductsLoaded;

    public async Task<OperationResult> AddAsync(Product product)
    {
        if (product is null) return OperationResult.Fail("Product is null");

        await using var uow = uowFactory.Create();

        await uow.ProductRepository.AddAsync(product);
        await uow.CompleteAsync();

        Products.Add(product);
        ProductAdded?.Invoke(product);
        return OperationResult.Ok("Product added successfully");
    }

    public async Task<OperationResult> UpdateAsync(Product product)
    {
        if (product is null) return OperationResult.Fail("Product is null");

        await using var uow = uowFactory.Create();

        var existing = await uow.ProductRepository.GetByIdAsync(product.Id);
        if (existing is null) return OperationResult.Fail("Product not found");

        uow.ProductRepository.Update(product);
        await uow.CompleteAsync();

        var index = Products.FindIndex(p => p.Id == product.Id);
        if (index >= 0) Products[index] = product;
        ProductUpdated?.Invoke(product);
        return OperationResult.Ok("Product updated successfully");
    }

    public async Task<OperationResult> DeleteAsync(Guid id)
    {
        await using var uow = uowFactory.Create();

        await uow.ProductRepository.DeleteAsync(id);
        await uow.CompleteAsync();

        Products.RemoveAll(p => p.Id == id);
        ProductDeleted?.Invoke(id);
        return OperationResult.Ok("Product deleted successfully");
    }

    public async Task LoadAllAsync()
    {
        await using var uow = uowFactory.Create();
        Products = (await uow.ProductRepository.GetAllAsync()).ToList();
        ProductsLoaded?.Invoke();
    }

    public async Task LoadPagedAsync(string? search, ProductSortBy sortBy, int page, int pageSize)
    {
        await using var uow = uowFactory.Create();
        CurrentPage = await uow.ProductRepository.GetPagedAsync(search, sortBy, page, pageSize);
        Products = CurrentPage.Items.ToList();
        ProductsLoaded?.Invoke();
    }
}
