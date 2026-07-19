using Microsoft.Extensions.DependencyInjection;
using TestList;

namespace Tulo.IntegrationTests.Services;

[TestClass]
public class SellerServiceTests
{
    private TestSystem _system = null!;
    private IServiceScope _scope = null!;

    [TestInitialize]
    public void Init()
    {
        _system = new TestSystem();
        _scope = _system.CreateTestServiceScope();
        _system.SetRequiredServices(_scope);
        _system.ClearDatabase();
    }

    [TestMethod]
    public async Task AddAsync_ShouldPersistSellerInDb()
    {
        var seller = new SellerFaker().Generate();

        var result = await _system.SellerService.AddAsync(seller);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _system.CreateDbContext();
        var fromDb = ctx.Sellers.FirstOrDefault(s => s.Id == seller.Id);
        Assert.IsNotNull(fromDb);
        Assert.AreEqual(seller.Name, fromDb.Name);
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldUpdateSellerInDb()
    {
        var seller = new SellerFaker().Generate();
        await _system.SellerService.AddAsync(seller);

        seller.Name = "Updated GmbH";
        var result = await _system.SellerService.UpdateAsync(seller);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _system.CreateDbContext();
        var fromDb = ctx.Sellers.FirstOrDefault(s => s.Id == seller.Id);
        Assert.IsNotNull(fromDb);
        Assert.AreEqual("Updated GmbH", fromDb.Name);
    }

    [TestMethod]
    public async Task DeleteAsync_ShouldRemoveSellerFromDb()
    {
        var seller = new SellerFaker().Generate();
        await _system.SellerService.AddAsync(seller);

        var result = await _system.SellerService.DeleteAsync(seller.Id);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _system.CreateDbContext();
        var fromDb = ctx.Sellers.FirstOrDefault(s => s.Id == seller.Id);
        Assert.IsNull(fromDb);
    }

    [TestMethod]
    public async Task LoadAsync_ShouldLoadSellerIntoService()
    {
        var seller = new SellerFaker().Generate();
        await _system.SellerService.AddAsync(seller);

        await _system.SellerService.LoadAsync();

        Assert.IsNotNull(_system.SellerService.Seller);
        Assert.AreEqual(seller.Name, _system.SellerService.Seller!.Name);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _system.ClearDatabase();
        _scope.Dispose();
    }
}
