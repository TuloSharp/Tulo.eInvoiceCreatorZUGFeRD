using Microsoft.Extensions.DependencyInjection;
using TestList;
using Tulo.Application.Enums;

namespace Tulo.IntegrationTests.Services;
[TestClass]
public class ProductServiceTests
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
    public async Task AddAsync_ShouldPersistProductInDb()
    {
        var product = new ProductFaker().Generate();

        var result = await _testSystem.ProductService.AddAsync(product);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _testSystem.CreateDbContext();
        var fromDb = ctx.Products.FirstOrDefault(p => p.Id == product.Id);
        Assert.IsNotNull(fromDb);
        Assert.AreEqual(product.Description, fromDb.Description);
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldUpdateProductInDb()
    {
        var product = new ProductFaker().Generate();
        await _testSystem.ProductService.AddAsync(product);

        product.Description = "Updated Product";
        var result = await _testSystem.ProductService.UpdateAsync(product);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _testSystem.CreateDbContext();
        var fromDb = ctx.Products.FirstOrDefault(p => p.Id == product.Id);
        Assert.IsNotNull(fromDb);
        Assert.AreEqual("Updated Product", fromDb.Description);
    }

    [TestMethod]
    public async Task DeleteAsync_ShouldRemoveProductFromDb()
    {
        var product = new ProductFaker().Generate();
        await _testSystem.ProductService.AddAsync(product);

        var result = await _testSystem.ProductService.DeleteAsync(product.Id);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _testSystem.CreateDbContext();
        var fromDb = ctx.Products.FirstOrDefault(p => p.Id == product.Id);
        Assert.IsNull(fromDb);
    }

    [TestMethod]
    public async Task LoadAllAsync_ShouldLoadAllProducts()
    {
        var products = new ProductFaker().Generate(5);
        foreach (var p in products)
            await _testSystem.ProductService.AddAsync(p);

        await _testSystem.ProductService.LoadAllAsync();

        Assert.AreEqual(5, _testSystem.ProductService.Products.Count);
    }

    [TestMethod]
    public async Task LoadPagedAsync_ShouldReturnCorrectPage()
    {
        var products = new ProductFaker().Generate(10);
        foreach (var p in products)
            await _testSystem.ProductService.AddAsync(p);

        await _testSystem.ProductService.LoadPagedAsync(null, ProductSortBy.Description, 1, 5);

        Assert.AreEqual(5, _testSystem.ProductService.CurrentPage.Items.Count);
        Assert.AreEqual(10, _testSystem.ProductService.CurrentPage.TotalCount);
        Assert.AreEqual(2, _testSystem.ProductService.CurrentPage.TotalPages);
    }

    [TestMethod]
    public async Task LoadPagedAsync_ShouldFilterBySearchTerm()
    {
        var products = new ProductFaker().Generate(5);
        products[0].Description = "Spezialartikel XYZ";

        foreach (var p in products)
            await _testSystem.ProductService.AddAsync(p);

        await _testSystem.ProductService.LoadPagedAsync("Spezialartikel", ProductSortBy.Description, 1, 10);

        Assert.AreEqual(1, _testSystem.ProductService.CurrentPage.TotalCount);
        Assert.AreEqual("Spezialartikel XYZ", _testSystem.ProductService.CurrentPage.Items[0].Description);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _testSystem.ClearTestSystem();
        _scope.Dispose();
    }
}

