using Microsoft.Extensions.DependencyInjection;
using TestList;
using Tulo.Application.Enums;
using Tulo.Domain.Entitites;

namespace Tulo.IntegrationTests.Services;
[TestClass]
public class InvoiceHeaderServiceTests
{
    private TestSystem _testSystem = null!;
    private IServiceScope _scope = null!;
    private Customer _customer = null!;
    private Seller _seller = null!;

    [TestInitialize]
    public async Task Init()
    {
        _testSystem = new TestSystem();
        _testSystem.ClearTestSystem();
        _scope = _testSystem.CreateTestServiceScope();
        _testSystem.SetRequiredServices(_scope);

        // InvoiceHeader braucht gültige FK auf Customer und Seller
        _customer = new CustomerFaker().Generate();
        _seller = new SellerFaker().Generate();
        await _testSystem.CustomerService.AddAsync(_customer);
        await _testSystem.SellerService.AddAsync(_seller);
    }

    [TestMethod]
    public async Task AddAsync_ShouldPersistInvoiceHeaderInDb()
    {
        var invoice = new InvoiceHeaderFaker(_customer.Id, _seller.Id).Generate();

        var result = await _testSystem.InvoiceHeaderService.AddAsync(invoice);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _testSystem.CreateDbContext();
        var fromDb = ctx.InvoiceHeaders.FirstOrDefault(i => i.Id == invoice.Id);
        Assert.IsNotNull(fromDb);
        Assert.AreEqual(invoice.InvoiceNumber, fromDb.InvoiceNumber);
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldUpdateInvoiceHeaderInDb()
    {
        var invoice = new InvoiceHeaderFaker(_customer.Id, _seller.Id).Generate();
        await _testSystem.InvoiceHeaderService.AddAsync(invoice);

        invoice.FileName = "RE-2024-001.pdf";
        var result = await _testSystem.InvoiceHeaderService.UpdateAsync(invoice);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _testSystem.CreateDbContext();
        var fromDb = ctx.InvoiceHeaders.FirstOrDefault(i => i.Id == invoice.Id);
        Assert.IsNotNull(fromDb);
        Assert.AreEqual("RE-2024-001.pdf", fromDb.FileName);
    }

    [TestMethod]
    public async Task DeleteAsync_ShouldRemoveInvoiceHeaderFromDb()
    {
        var invoice = new InvoiceHeaderFaker(_customer.Id, _seller.Id).Generate();
        await _testSystem.InvoiceHeaderService.AddAsync(invoice);

        var result = await _testSystem.InvoiceHeaderService.DeleteAsync(invoice.Id);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _testSystem.CreateDbContext();
        var fromDb = ctx.InvoiceHeaders.FirstOrDefault(i => i.Id == invoice.Id);
        Assert.IsNull(fromDb);
    }

    [TestMethod]
    public async Task LoadAllAsync_ShouldLoadAllInvoiceHeaders()
    {
        var invoices = new InvoiceHeaderFaker(_customer.Id, _seller.Id).Generate(5);
        foreach (var i in invoices)
            await _testSystem.InvoiceHeaderService.AddAsync(i);

        await _testSystem.InvoiceHeaderService.LoadAllAsync();

        Assert.AreEqual(5, _testSystem.InvoiceHeaderService.InvoiceHeaders.Count);
    }

    [TestMethod]
    public async Task LoadPagedAsync_ShouldReturnCorrectPage()
    {
        var invoices = new InvoiceHeaderFaker(_customer.Id, _seller.Id).Generate(10);
        foreach (var i in invoices)
            await _testSystem.InvoiceHeaderService.AddAsync(i);

        await _testSystem.InvoiceHeaderService.LoadPagedAsync(null, InvoiceHeaderSortBy.InvoiceDate, 1, 5, null, null);

        Assert.AreEqual(5, _testSystem.InvoiceHeaderService.CurrentPage.Items.Count);
        Assert.AreEqual(10, _testSystem.InvoiceHeaderService.CurrentPage.TotalCount);
        Assert.AreEqual(2, _testSystem.InvoiceHeaderService.CurrentPage.TotalPages);
    }

    [TestMethod]
    public async Task LoadPagedAsync_ShouldFilterByDateRange()
    {
        var oldInvoice = new InvoiceHeaderFaker(_customer.Id, _seller.Id).Generate();
        oldInvoice.InvoiceDate = new DateOnly(2023, 1, 1);
        await _testSystem.InvoiceHeaderService.AddAsync(oldInvoice);

        var recentInvoice = new InvoiceHeaderFaker(_customer.Id, _seller.Id).Generate();
        recentInvoice.InvoiceDate = new DateOnly(2025, 6, 1);
        await _testSystem.InvoiceHeaderService.AddAsync(recentInvoice);

        await _testSystem.InvoiceHeaderService.LoadPagedAsync(
            null, InvoiceHeaderSortBy.InvoiceDate, 1, 10,
            dateFrom: new DateOnly(2025, 1, 1),
            dateTo: new DateOnly(2025, 12, 31));

        Assert.AreEqual(1, _testSystem.InvoiceHeaderService.CurrentPage.TotalCount);
        Assert.AreEqual(recentInvoice.Id, _testSystem.InvoiceHeaderService.CurrentPage.Items[0].Id);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _testSystem.ClearTestSystem();
        _scope.Dispose();
    }
}

