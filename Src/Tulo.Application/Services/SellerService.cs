using tulo.CoreLib.Components.ResultPattern;
using Tulo.Application.Interfaces.Services;
using Tulo.Application.Interfaces.UnitOfWorks;
using Tulo.Domain.Entitites;

namespace Tulo.Application.Services;

public class SellerService(IUnitOfWorkFactory uowFactory) : ISellerService
{
    public Seller? Seller { get; private set; }

    public event Action? SellerLoaded;
    public event Action<Seller>? SellerCreated;
    public event Action<Seller>? SellerUpdated;
    public event Action<Guid>? SellerDeleted;

    public async Task<OperationResult> AddAsync(Seller seller)
    {
        if (seller is null) return OperationResult.Fail("Seller is null");

        await using var uow = uowFactory.Create();
        await uow.SellerRepository.AddAsync(seller);
        await uow.CompleteAsync();

        Seller = seller;
        SellerCreated?.Invoke(seller);
        return OperationResult.Ok("Seller created successfully");
    }

    public async Task<OperationResult> UpdateAsync(Seller seller)
    {
        if (seller is null) return OperationResult.Fail("Seller is null");

        await using var uow = uowFactory.Create();

        var existing = await uow.SellerRepository.GetAsync();
        if (existing is null) return OperationResult.Fail("Seller not found");

        uow.SellerRepository.Update(seller);
        await uow.CompleteAsync();

        Seller = seller;
        SellerUpdated?.Invoke(seller);
        return OperationResult.Ok("Seller updated successfully");
    }

    public async Task<OperationResult> DeleteAsync(Guid id)
    {
        await using var uow = uowFactory.Create();

        await uow.SellerRepository.DeleteAsync(id);
        await uow.CompleteAsync();

        Seller = null;
        SellerDeleted?.Invoke(id);
        return OperationResult.Ok("Seller deleted successfully");
    }

    public async Task LoadAsync()
    {
        await using var uow = uowFactory.Create();
        Seller = await uow.SellerRepository.GetAsync();
        SellerLoaded?.Invoke();
    }
}
