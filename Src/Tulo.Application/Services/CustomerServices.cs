using tulo.CoreLib.Components.ResultPattern;
using Tulo.Application.Enums;
using Tulo.Application.Interfaces.Services;
using Tulo.Application.Interfaces.UnitOfWorks;
using Tulo.Domain.Entitites;

namespace Tulo.Application.Services;

public class CustomerService(IUnitOfWorkFactory uowFactory) : ICustomerService
{
    public List<Customer> Customers { get; private set; } = [];

    public event Action? CustomersLoaded;
    public event Action<Customer>? CustomerCreated;
    public event Action<Customer>? CustomerUpdated;
    public event Action<Guid>? CustomerDeleted;

    public async Task<OperationResult> AddAsync(Customer customer)
    {
        if (customer is null) return OperationResult.Fail("Customer is null");

        await using var uow = uowFactory.Create();
        await uow.CustomerRepository.AddAsync(customer);
        await uow.CompleteAsync();

        Customers.Add(customer);
        CustomerCreated?.Invoke(customer);
        return OperationResult.Ok("Customer created successfully");
    }

    public async Task<OperationResult> UpdateAsync(Customer customer)
    {
        if (customer is null) return OperationResult.Fail("Customer is null");

        await using var uow = uowFactory.Create();

        var existing = await uow.CustomerRepository.GetByIdAsync(customer.Id);
        if (existing is null) return OperationResult.Fail("Customer not found");

        uow.CustomerRepository.Update(customer);
        await uow.CompleteAsync();

        var index = Customers.FindIndex(c => c.Id == customer.Id);
        if (index >= 0) Customers[index] = customer;
        CustomerUpdated?.Invoke(customer);
        return OperationResult.Ok("Customer updated successfully");
    }

    public async Task<OperationResult> DeleteAsync(Guid id)
    {
        await using var uow = uowFactory.Create();

        await uow.CustomerRepository.DeleteAsync(id);
        await uow.CompleteAsync();

        Customers.RemoveAll(c => c.Id == id);
        CustomerDeleted?.Invoke(id);
        return OperationResult.Ok("Customer deleted successfully");
    }

    public async Task LoadAllAsync()
    {
        await using var uow = uowFactory.Create();
        Customers = (await uow.CustomerRepository.GetAllAsync()).ToList();
        CustomersLoaded?.Invoke();
    }

    public async Task LoadFilteredAsync(string? search, CustomerSortBy sortBy)
    {
        await using var uow = uowFactory.Create();
        Customers = (await uow.CustomerRepository.GetFilteredSortedAsync(search, sortBy)).ToList();
        CustomersLoaded?.Invoke();
    }
}
