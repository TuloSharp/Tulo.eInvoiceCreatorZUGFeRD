using Microsoft.Extensions.DependencyInjection;
using TestList;

namespace Tulo.IntegrationTests.Services;

[TestClass]
public class CustomerServiceTests
{
    private TestSystem _testSystem = null!;
    private IServiceScope _scope = null!;

    [TestInitialize]
    public void Init()
    {
        _testSystem = new TestSystem();
        _testSystem.ClearTestSystem();

        _scope = _testSystem.CreateTestServiceScope();
        _testSystem.SetRequiredServices(_scope);
    }

    [TestMethod]
    public async Task AddAsync_ShouldPersistCustomerInDb()
    {
        var customer = new CustomerFaker().Generate();

        var result = await _testSystem.CustomerService.AddAsync(customer);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _testSystem.CreateDbContext();
        var fromDb = ctx.Customers.FirstOrDefault(c => c.Id == customer.Id);
        Assert.IsNotNull(fromDb);
        Assert.AreEqual(customer.Name, fromDb.Name);
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldUpdateCustomerInDb()
    {
        var customer = new CustomerFaker().Generate();
        await _testSystem.CustomerService.AddAsync(customer);

        customer.Name = "Updated Kunde GmbH";
        var result = await _testSystem.CustomerService.UpdateAsync(customer);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _testSystem.CreateDbContext();
        var fromDb = ctx.Customers.FirstOrDefault(c => c.Id == customer.Id);
        Assert.IsNotNull(fromDb);
        Assert.AreEqual("Updated Kunde GmbH", fromDb.Name);
    }

    [TestMethod]
    public async Task DeleteAsync_ShouldRemoveCustomerFromDb()
    {
        var customer = new CustomerFaker().Generate();
        await _testSystem.CustomerService.AddAsync(customer);

        var result = await _testSystem.CustomerService.DeleteAsync(customer.Id);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _testSystem.CreateDbContext();
        var fromDb = ctx.Customers.FirstOrDefault(c => c.Id == customer.Id);
        Assert.IsNull(fromDb);
    }

    [TestMethod]
    public async Task LoadAllAsync_ShouldLoadAllCustomers()
    {
        var customers = new CustomerFaker().Generate(5);
        foreach (var c in customers)
            await _testSystem.CustomerService.AddAsync(c);

        await _testSystem.CustomerService.LoadAllAsync();

        Assert.AreEqual(5, _testSystem.CustomerService.Customers.Count);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _testSystem.ClearTestSystem();
        _scope.Dispose();
    }
}
