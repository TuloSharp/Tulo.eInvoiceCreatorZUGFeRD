using Microsoft.Extensions.DependencyInjection;
using TestList;
using Tulo.Application.DTOs;
using Tulo.Domain.Entitites;

namespace Tulo.IntegrationTests.Services;
[TestClass]
public class InvoicePositionServiceTests
{
    private TestSystem _testSystem = null!;
    private IServiceScope _scope = null!;
    private InvoiceHeader _invoice = null!;

    [TestInitialize]
    public async Task Init()
    {
        _testSystem = new TestSystem();
        _testSystem.ClearTestSystem();
        _scope = _testSystem.CreateTestServiceScope();
        _testSystem.SetRequiredServices(_scope);

        var customer = new CustomerFaker().Generate();
        var seller = new SellerFaker().Generate();
        await _testSystem.CustomerService.AddAsync(customer);
        await _testSystem.SellerService.AddAsync(seller);

        _invoice = new InvoiceHeaderFaker(customer.Id, seller.Id).Generate();
        await _testSystem.InvoiceHeaderService.AddAsync(_invoice);
    }

    [TestMethod]
    public async Task AddAsync_ShouldPersistInvoicePositionInDb()
    {
        var position = new InvoicePositionFaker(_invoice.Id).Generate();

        var result = await _testSystem.InvoicePositionService.AddAsync(position);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _testSystem.CreateDbContext();
        var fromDb = ctx.InvoicePositions.FirstOrDefault(p => p.Id == position.Id);
        Assert.IsNotNull(fromDb);
        Assert.AreEqual(position.Description, fromDb.Description);
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldUpdateInvoicePositionInDb()
    {
        var position = new InvoicePositionFaker(_invoice.Id).Generate();
        await _testSystem.InvoicePositionService.AddAsync(position);

        position.Description = "Updated Position";
        var result = await _testSystem.InvoicePositionService.UpdateAsync(position);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _testSystem.CreateDbContext();
        var fromDb = ctx.InvoicePositions.FirstOrDefault(p => p.Id == position.Id);
        Assert.IsNotNull(fromDb);
        Assert.AreEqual("Updated Position", fromDb.Description);
    }

    [TestMethod]
    public async Task DeleteAsync_ShouldRemoveInvoicePositionFromDb()
    {
        var position = new InvoicePositionFaker(_invoice.Id).Generate();
        await _testSystem.InvoicePositionService.AddAsync(position);

        var result = await _testSystem.InvoicePositionService.DeleteAsync(position.Id);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _testSystem.CreateDbContext();
        var fromDb = ctx.InvoicePositions.FirstOrDefault(p => p.Id == position.Id);
        Assert.IsNull(fromDb);
    }

    [TestMethod]
    public async Task LoadByInvoiceIdAsync_ShouldLoadOnlyPositionsForGivenInvoice()
    {
        var positions = new InvoicePositionFaker(_invoice.Id).Generate(3);
        foreach (var p in positions)
            await _testSystem.InvoicePositionService.AddAsync(p);

        await _testSystem.InvoicePositionService.LoadByInvoiceIdAsync(_invoice.Id);

        Assert.AreEqual(3, _testSystem.InvoicePositionService.Positions.Count);
        Assert.IsTrue(_testSystem.InvoicePositionService.Positions.All(p => p.InvoiceId == _invoice.Id));
    }

    [TestMethod]
    public async Task DeleteAllByInvoiceIdAsync_ShouldRemoveAllPositionsForInvoice()
    {
        var positions = new InvoicePositionFaker(_invoice.Id).Generate(4);
        foreach (var p in positions)
            await _testSystem.InvoicePositionService.AddAsync(p);

        var result = await _testSystem.InvoicePositionService.DeleteAllByInvoiceIdAsync(_invoice.Id);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _testSystem.CreateDbContext();
        var remaining = ctx.InvoicePositions.Where(p => p.InvoiceId == _invoice.Id).ToList();
        Assert.AreEqual(0, remaining.Count);
    }

    [TestMethod]
    public async Task LoadPagedByInvoiceIdAsync_ShouldReturnCorrectPage()
    {
        var positions = new InvoicePositionFaker(_invoice.Id).Generate(10);
        foreach (var p in positions)
            await _testSystem.InvoicePositionService.AddAsync(p);

        await _testSystem.InvoicePositionService.LoadPagedByInvoiceIdAsync(_invoice.Id, 1, 5);

        Assert.AreEqual(5, _testSystem.InvoicePositionService.CurrentPage.Items.Count);
        Assert.AreEqual(10, _testSystem.InvoicePositionService.CurrentPage.TotalCount);
        Assert.AreEqual(2, _testSystem.InvoicePositionService.CurrentPage.TotalPages);
    }

    // ── SuggestNextPositionNoAsync ─────────────────────────────────────────────

    [TestMethod]
    public async Task SuggestNextPositionNoAsync_ShouldReturnOneWhenNoPositionsExist()
    {
        var result = await _testSystem.InvoicePositionService.SuggestNextPositionNoAsync(_invoice.Id);

        Assert.AreEqual(1, result);
    }

    [TestMethod]
    public async Task SuggestNextPositionNoAsync_ShouldReturnNextAfterExistingTopLevelPositions()
    {
        var dto1 = CreateDto("Pos 1");
        var dto2 = CreateDto("Pos 2");
        await _testSystem.InvoicePositionService.AddInvoicePositionAsync(_invoice.Id, dto1);
        await _testSystem.InvoicePositionService.AddInvoicePositionAsync(_invoice.Id, dto2);

        var result = await _testSystem.InvoicePositionService.SuggestNextPositionNoAsync(_invoice.Id);

        Assert.AreEqual(3, result);
    }

    // ── SuggestNextSubPositionNoAsync ──────────────────────────────────────────

    [TestMethod]
    public async Task SuggestNextSubPositionNoAsync_ShouldReturnOneWhenNoChildrenExist()
    {
        var groupDto = CreateDto("Group", lineStatusReasonCode: "GROUP");
        var groupResult = await _testSystem.InvoicePositionService.AddInvoicePositionAsync(_invoice.Id, groupDto);

        var result = await _testSystem.InvoicePositionService
            .SuggestNextSubPositionNoAsync(_invoice.Id, groupResult.Data);

        Assert.AreEqual(1, result);
    }

    [TestMethod]
    public async Task SuggestNextSubPositionNoAsync_ShouldReturnNextAfterExistingChildren()
    {
        var groupDto = CreateDto("Group", lineStatusReasonCode: "GROUP");
        var groupResult = await _testSystem.InvoicePositionService.AddInvoicePositionAsync(_invoice.Id, groupDto);
        var parentId = groupResult.Data;

        await _testSystem.InvoicePositionService.AddSubInvoicePositionAsync(_invoice.Id, parentId, CreateDto("Sub 1"));
        await _testSystem.InvoicePositionService.AddSubInvoicePositionAsync(_invoice.Id, parentId, CreateDto("Sub 2"));

        var result = await _testSystem.InvoicePositionService
            .SuggestNextSubPositionNoAsync(_invoice.Id, parentId);

        Assert.AreEqual(3, result);
    }

    // ── AddInvoicePositionAsync ────────────────────────────────────────────────

    [TestMethod]
    public async Task AddInvoicePositionAsync_ShouldPersistInDb()
    {
        var dto = CreateDto("New Position");

        var result = await _testSystem.InvoicePositionService.AddInvoicePositionAsync(_invoice.Id, dto);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _testSystem.CreateDbContext();
        var fromDb = ctx.InvoicePositions.FirstOrDefault(p => p.Id == result.Data);
        Assert.IsNotNull(fromDb);
        Assert.AreEqual("New Position", fromDb.Description);
        Assert.AreEqual(1, fromDb.SortOrder);
    }

    [TestMethod]
    public async Task AddInvoicePositionAsync_WithDesiredPositionNo_ShouldInsertAtCorrectPosition()
    {
        // Arrange: add 2 positions first
        var result1 = await _testSystem.InvoicePositionService
            .AddInvoicePositionAsync(_invoice.Id, CreateDto("First"));
        var result2 = await _testSystem.InvoicePositionService
            .AddInvoicePositionAsync(_invoice.Id, CreateDto("Second"));

        // Act: insert new position at positionNo=1
        var result3 = await _testSystem.InvoicePositionService
            .AddInvoicePositionAsync(_invoice.Id, CreateDto("Inserted"), desiredPositionNo: 1);

        Assert.IsTrue(result3.Success, result3.Message);

        using var ctx = _testSystem.CreateDbContext();
        var all = ctx.InvoicePositions
            .Where(p => p.InvoiceId == _invoice.Id)
            .OrderBy(p => p.SortOrder)
            .ToList();

        Assert.AreEqual(3, all.Count);
        Assert.AreEqual(result3.Data, all[0].Id);   // Inserted is first
        Assert.AreEqual(result1.Data, all[1].Id);   // First is now second
        Assert.AreEqual(result2.Data, all[2].Id);   // Second is now third
    }

    [TestMethod]
    public async Task AddInvoicePositionAsync_ShouldFirePositionAddedEvent()
    {
        InvoicePosition? firedWith = null;
        _testSystem.InvoicePositionService.PositionAdded += p => firedWith = p;

        await _testSystem.InvoicePositionService.AddInvoicePositionAsync(_invoice.Id, CreateDto("Pos"));

        Assert.IsNotNull(firedWith);
        Assert.AreEqual("Pos", firedWith.Description);
    }

    // ── AddSubInvoicePositionAsync ─────────────────────────────────────────────

    [TestMethod]
    public async Task AddSubInvoicePositionAsync_ShouldPersistWithCorrectParent()
    {
        var groupResult = await _testSystem.InvoicePositionService
            .AddInvoicePositionAsync(_invoice.Id, CreateDto("Group", lineStatusReasonCode: "GROUP"));
        var parentId = groupResult.Data;

        var subResult = await _testSystem.InvoicePositionService
            .AddSubInvoicePositionAsync(_invoice.Id, parentId, CreateDto("Sub"));

        Assert.IsTrue(subResult.Success, subResult.Message);

        using var ctx = _testSystem.CreateDbContext();
        var fromDb = ctx.InvoicePositions.FirstOrDefault(p => p.Id == subResult.Data);
        Assert.IsNotNull(fromDb);
        Assert.AreEqual(parentId, fromDb.ParentPositionId);
        Assert.AreEqual("DETAIL", fromDb.LineStatusReasonCode);
    }

    [TestMethod]
    public async Task AddSubInvoicePositionAsync_ShouldAppearBeforeGroupInSortOrder()
    {
        var groupResult = await _testSystem.InvoicePositionService
            .AddInvoicePositionAsync(_invoice.Id, CreateDto("Group", lineStatusReasonCode: "GROUP"));
        var parentId = groupResult.Data;

        var subResult = await _testSystem.InvoicePositionService
            .AddSubInvoicePositionAsync(_invoice.Id, parentId, CreateDto("Sub"));

        using var ctx = _testSystem.CreateDbContext();
        var group = ctx.InvoicePositions.First(p => p.Id == parentId);
        var sub = ctx.InvoicePositions.First(p => p.Id == subResult.Data);

        Assert.IsTrue(sub.SortOrder < group.SortOrder,
            "Sub-position must appear before its GROUP in SortOrder");
    }

    [TestMethod]
    public async Task AddSubInvoicePositionAsync_ShouldFailWhenParentIsNotGroup()
    {
        var standaloneResult = await _testSystem.InvoicePositionService
            .AddInvoicePositionAsync(_invoice.Id, CreateDto("Standalone"));

        var subResult = await _testSystem.InvoicePositionService
            .AddSubInvoicePositionAsync(_invoice.Id, standaloneResult.Data, CreateDto("Sub"));

        Assert.IsFalse(subResult.Success);
    }

    [TestMethod]
    public async Task AddSubInvoicePositionAsync_ShouldFailWhenParentNotFound()
    {
        var result = await _testSystem.InvoicePositionService
            .AddSubInvoicePositionAsync(_invoice.Id, Guid.NewGuid(), CreateDto("Sub"));

        Assert.IsFalse(result.Success);
    }

    // ── UpdateInvoicePositionAsync ─────────────────────────────────────────────

    [TestMethod]
    public async Task UpdateInvoicePositionAsync_ShouldUpdateBusinessFieldsInDb()
    {
        var addResult = await _testSystem.InvoicePositionService
            .AddInvoicePositionAsync(_invoice.Id, CreateDto("Old"));

        var updatedDto = CreateDto("Updated", unitPrice: 250, quantity: 3);
        var result = await _testSystem.InvoicePositionService
            .UpdateInvoicePositionAsync(_invoice.Id, addResult.Data, updatedDto);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _testSystem.CreateDbContext();
        var fromDb = ctx.InvoicePositions.First(p => p.Id == addResult.Data);
        Assert.AreEqual("Updated", fromDb.Description);
        Assert.AreEqual(250m, fromDb.UnitPriceNet);
        Assert.AreEqual(3m, fromDb.Quantity);
    }

    [TestMethod]
    public async Task UpdateInvoicePositionAsync_ShouldNotChangeSortOrder()
    {
        var addResult = await _testSystem.InvoicePositionService
            .AddInvoicePositionAsync(_invoice.Id, CreateDto("Pos"));

        using var ctxBefore = _testSystem.CreateDbContext();
        var sortOrderBefore = ctxBefore.InvoicePositions.First(p => p.Id == addResult.Data).SortOrder;

        await _testSystem.InvoicePositionService
            .UpdateInvoicePositionAsync(_invoice.Id, addResult.Data, CreateDto("Updated"));

        using var ctxAfter = _testSystem.CreateDbContext();
        var sortOrderAfter = ctxAfter.InvoicePositions.First(p => p.Id == addResult.Data).SortOrder;

        Assert.AreEqual(sortOrderBefore, sortOrderAfter);
    }

    [TestMethod]
    public async Task UpdateInvoicePositionAsync_ShouldFailWhenNotFound()
    {
        var result = await _testSystem.InvoicePositionService
            .UpdateInvoicePositionAsync(_invoice.Id, Guid.NewGuid(), CreateDto("X"));

        Assert.IsFalse(result.Success);
    }

    // ── DeleteInvoicePositionAsync ─────────────────────────────────────────────

    [TestMethod]
    public async Task DeleteInvoicePositionAsync_ShouldRemovePositionFromDb()
    {
        var addResult = await _testSystem.InvoicePositionService
            .AddInvoicePositionAsync(_invoice.Id, CreateDto("Pos"));

        var result = await _testSystem.InvoicePositionService
            .DeleteInvoicePositionAsync(_invoice.Id, addResult.Data);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _testSystem.CreateDbContext();
        Assert.IsNull(ctx.InvoicePositions.FirstOrDefault(p => p.Id == addResult.Data));
    }

    [TestMethod]
    public async Task DeleteInvoicePositionAsync_GroupShouldCascadeDeleteChildren()
    {
        var groupResult = await _testSystem.InvoicePositionService
            .AddInvoicePositionAsync(_invoice.Id, CreateDto("Group", lineStatusReasonCode: "GROUP"));
        var parentId = groupResult.Data;

        var sub1 = await _testSystem.InvoicePositionService
            .AddSubInvoicePositionAsync(_invoice.Id, parentId, CreateDto("Sub 1"));
        var sub2 = await _testSystem.InvoicePositionService
            .AddSubInvoicePositionAsync(_invoice.Id, parentId, CreateDto("Sub 2"));

        var result = await _testSystem.InvoicePositionService
            .DeleteInvoicePositionAsync(_invoice.Id, parentId);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _testSystem.CreateDbContext();
        Assert.IsNull(ctx.InvoicePositions.FirstOrDefault(p => p.Id == parentId));
        Assert.IsNull(ctx.InvoicePositions.FirstOrDefault(p => p.Id == sub1.Data));
        Assert.IsNull(ctx.InvoicePositions.FirstOrDefault(p => p.Id == sub2.Data));
    }

    [TestMethod]
    public async Task DeleteInvoicePositionAsync_ShouldRebuildSortOrderOfRemaining()
    {
        var r1 = await _testSystem.InvoicePositionService
            .AddInvoicePositionAsync(_invoice.Id, CreateDto("Pos 1"));
        var r2 = await _testSystem.InvoicePositionService
            .AddInvoicePositionAsync(_invoice.Id, CreateDto("Pos 2"));
        var r3 = await _testSystem.InvoicePositionService
            .AddInvoicePositionAsync(_invoice.Id, CreateDto("Pos 3"));

        // Delete the middle one
        await _testSystem.InvoicePositionService.DeleteInvoicePositionAsync(_invoice.Id, r2.Data);

        using var ctx = _testSystem.CreateDbContext();
        var remaining = ctx.InvoicePositions
            .Where(p => p.InvoiceId == _invoice.Id)
            .OrderBy(p => p.SortOrder)
            .ToList();

        Assert.AreEqual(2, remaining.Count);
        Assert.AreEqual(1, remaining[0].SortOrder);
        Assert.AreEqual(2, remaining[1].SortOrder);
    }

    [TestMethod]
    public async Task DeleteInvoicePositionAsync_ShouldFailWhenNotFound()
    {
        var result = await _testSystem.InvoicePositionService
            .DeleteInvoicePositionAsync(_invoice.Id, Guid.NewGuid());

        Assert.IsFalse(result.Success);
    }

    // ── SetInvoicePositionNoAsync ──────────────────────────────────────────────

    [TestMethod]
    public async Task SetInvoicePositionNoAsync_ShouldMovePositionToNewSpot()
    {
        var r1 = await _testSystem.InvoicePositionService
            .AddInvoicePositionAsync(_invoice.Id, CreateDto("Pos 1"));
        var r2 = await _testSystem.InvoicePositionService
            .AddInvoicePositionAsync(_invoice.Id, CreateDto("Pos 2"));
        var r3 = await _testSystem.InvoicePositionService
            .AddInvoicePositionAsync(_invoice.Id, CreateDto("Pos 3"));

        // Move Pos 3 to position 1
        var result = await _testSystem.InvoicePositionService
            .SetInvoicePositionNoAsync(_invoice.Id, r3.Data, 1);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _testSystem.CreateDbContext();
        var all = ctx.InvoicePositions
            .Where(p => p.InvoiceId == _invoice.Id)
            .OrderBy(p => p.SortOrder)
            .ToList();

        Assert.AreEqual(r3.Data, all[0].Id);
        Assert.AreEqual(r1.Data, all[1].Id);
        Assert.AreEqual(r2.Data, all[2].Id);
    }

    [TestMethod]
    public async Task SetInvoicePositionNoAsync_ShouldReturnDtosWithCorrectLineIds()
    {
        var r1 = await _testSystem.InvoicePositionService
            .AddInvoicePositionAsync(_invoice.Id, CreateDto("Pos 1"));
        var r2 = await _testSystem.InvoicePositionService
            .AddInvoicePositionAsync(_invoice.Id, CreateDto("Pos 2"));
        var r3 = await _testSystem.InvoicePositionService
            .AddInvoicePositionAsync(_invoice.Id, CreateDto("Pos 3"));

        // Move Pos 3 to position 1
        var result = await _testSystem.InvoicePositionService
            .SetInvoicePositionNoAsync(_invoice.Id, r3.Data, 1);

        Assert.IsTrue(result.Success, result.Message);

        var dtos = result.Data;
        Assert.AreEqual("01", dtos[0].LineId);
        Assert.AreEqual("02", dtos[1].LineId);
        Assert.AreEqual("03", dtos[2].LineId);
        Assert.AreEqual(1, dtos[0].InvoicePositionNr);
        Assert.AreEqual(2, dtos[1].InvoicePositionNr);
        Assert.AreEqual(3, dtos[2].InvoicePositionNr);
    }

    [TestMethod]
    public async Task SetInvoicePositionNoAsync_ShouldMoveGroupWithChildren()
    {
        var groupResult = await _testSystem.InvoicePositionService
            .AddInvoicePositionAsync(_invoice.Id, CreateDto("Group", lineStatusReasonCode: "GROUP"));
        var parentId = groupResult.Data;

        var subResult = await _testSystem.InvoicePositionService
            .AddSubInvoicePositionAsync(_invoice.Id, parentId, CreateDto("Sub"));

        var standalone = await _testSystem.InvoicePositionService
            .AddInvoicePositionAsync(_invoice.Id, CreateDto("Standalone"));

        // Move Group (currently pos 1) to pos 2
        var result = await _testSystem.InvoicePositionService
            .SetInvoicePositionNoAsync(_invoice.Id, parentId, 2);

        Assert.IsTrue(result.Success, result.Message);

        using var ctx = _testSystem.CreateDbContext();
        var all = ctx.InvoicePositions
            .Where(p => p.InvoiceId == _invoice.Id)
            .OrderBy(p => p.SortOrder)
            .ToList();

        // Standalone should now be first, then Sub, then Group
        Assert.AreEqual(standalone.Data, all[0].Id);
        Assert.AreEqual(subResult.Data, all[1].Id);
        Assert.AreEqual(parentId, all[2].Id);
    }

    [TestMethod]
    public async Task SetInvoicePositionNoAsync_ShouldFailForDetailPosition()
    {
        var groupResult = await _testSystem.InvoicePositionService
            .AddInvoicePositionAsync(_invoice.Id, CreateDto("Group", lineStatusReasonCode: "GROUP"));

        var subResult = await _testSystem.InvoicePositionService
            .AddSubInvoicePositionAsync(_invoice.Id, groupResult.Data, CreateDto("Sub"));

        var result = await _testSystem.InvoicePositionService
            .SetInvoicePositionNoAsync(_invoice.Id, subResult.Data, 1);

        Assert.IsFalse(result.Success);
    }

    // ── TotalCount ─────────────────────────────────────────────────────────────

    [TestMethod]
    public async Task TotalCount_ShouldReflectLoadedPositions()
    {
        var positions = new InvoicePositionFaker(_invoice.Id).Generate(5);
        foreach (var p in positions)
            await _testSystem.InvoicePositionService.AddAsync(p);

        await _testSystem.InvoicePositionService.LoadByInvoiceIdAsync(_invoice.Id);

        Assert.AreEqual(5, _testSystem.InvoicePositionService.TotalCount);
    }

    // ── Cleanup ───────────────────────────────────────────────────────────────

    [TestCleanup]
    public void Cleanup()
    {
        _testSystem.ClearTestSystem();
        _scope.Dispose();
    }

    // ── Helper ────────────────────────────────────────────────────────────────

    private static InvoicePositionDetailsDTO CreateDto(string description = "Test Position",
                                                       string lineStatusReasonCode = "",
                                                       Guid? parentPositionId = null,
                                                       decimal unitPrice = 100,
                                                       decimal quantity = 1) => new()
        {
            InvoicePositionDescription = description,
            InvoicePositionQuantity = quantity,
            InvoicePostionUnit = "C62",
            InvoicePositionUnitPrice = unitPrice,
            InvoicePositionVatRate = 19,
            InvoicePositionVatCategoryCode = "S",
            LineStatusReasonCode = lineStatusReasonCode,
            ParentPositionId = parentPositionId,
        };

}
