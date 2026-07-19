using tulo.CoreLib.Components.ResultPattern;
using Tulo.Application.Enums;
using Tulo.Domain.Entitites;

namespace Tulo.Application.Interfaces.Services;

public interface ICustomerService
{
    List<Customer> Customers { get; }

    event Action? CustomersLoaded;
    event Action<Customer>? CustomerCreated;
    event Action<Customer>? CustomerUpdated;
    event Action<Guid>? CustomerDeleted;

    Task<OperationResult> AddAsync(Customer customer);
    Task<OperationResult> UpdateAsync(Customer customer);
    Task<OperationResult> DeleteAsync(Guid id);
    Task LoadAllAsync();
    Task LoadFilteredAsync(string? search, CustomerSortBy sortBy);
}
