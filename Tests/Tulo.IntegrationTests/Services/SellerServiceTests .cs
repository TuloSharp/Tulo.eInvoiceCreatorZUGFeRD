using Microsoft.Extensions.DependencyInjection;
using TestList;

namespace Tulo.IntegrationTests.Services;

[TestClass]
public class SellerServiceTests
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
    public async Task AddAsync_ShouldPersistSellerInDb()
    {
        var seller = new SellerFaker().Generate();

        var result = await _testSystem.SellerService.AddAsync(seller);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _testSystem.CreateDbContext();
        var fromDb = ctx.Sellers.FirstOrDefault(s => s.Id == seller.Id);
        Assert.IsNotNull(fromDb);
        Assert.AreEqual(seller.Name, fromDb.Name);
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldUpdateSellerInDb()
    {
        var seller = new SellerFaker().Generate();
        await _testSystem.SellerService.AddAsync(seller);

        seller.Name = "Updated GmbH";
        var result = await _testSystem.SellerService.UpdateAsync(seller);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _testSystem.CreateDbContext();
        var fromDb = ctx.Sellers.FirstOrDefault(s => s.Id == seller.Id);
        Assert.IsNotNull(fromDb);
        Assert.AreEqual("Updated GmbH", fromDb.Name);
    }

    [TestMethod]
    public async Task DeleteAsync_ShouldRemoveSellerFromDb()
    {
        var seller = new SellerFaker().Generate();
        await _testSystem.SellerService.AddAsync(seller);

        var result = await _testSystem.SellerService.DeleteAsync(seller.Id);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _testSystem.CreateDbContext();
        var fromDb = ctx.Sellers.FirstOrDefault(s => s.Id == seller.Id);
        Assert.IsNull(fromDb);
    }

    [TestMethod]
    public async Task LoadAsync_ShouldLoadSellerIntoService()
    {
        var seller = new SellerFaker().Generate();
        await _testSystem.SellerService.AddAsync(seller);

        await _testSystem.SellerService.LoadAsync();

        Assert.IsNotNull(_testSystem.SellerService.Seller);
        Assert.AreEqual(seller.Name, _testSystem.SellerService.Seller!.Name);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _testSystem.ClearTestSystem();
        _scope.Dispose();
    }
}
