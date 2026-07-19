using tulo.CoreLib.Components.ResultPattern;
using Tulo.Domain.Entitites;

namespace Tulo.Application.Interfaces.Services;

public interface ISellerService
{
    Seller? Seller { get; }

    event Action? SellerLoaded;
    event Action<Seller>? SellerCreated;
    event Action<Seller>? SellerUpdated;
    event Action<Guid>? SellerDeleted;

    Task<OperationResult> AddAsync(Seller seller);
    Task<OperationResult> UpdateAsync(Seller seller);
    Task<OperationResult> DeleteAsync(Guid id);
    Task LoadAsync();
}
