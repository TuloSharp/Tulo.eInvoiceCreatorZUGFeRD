using tulo.CoreLib.Components.ResultPattern;
using Tulo.Application.DTOs;
using Tulo.Application.Interfaces.Services;
using Tulo.Application.Interfaces.UnitOfWorks;
using Tulo.Application.ResultPattern;
using Tulo.Domain.Entitites;

namespace Tulo.Application.Services;
public sealed class InvoicePositionService(IUnitOfWorkFactory uowFactory) : IInvoicePositionService
{
    public List<InvoicePosition> Positions { get; private set; } = [];
    public PagedResult<InvoicePosition> CurrentPage { get; private set; } = new();
    public int TotalCount => Positions.Count;

    public event Action<InvoicePosition>? PositionAdded;
    public event Action<InvoicePosition>? PositionUpdated;
    public event Action<Guid>? PositionDeleted;
    public event Action? PositionsLoaded;

    // ── Existing ──────────────────────────────────────────────────────────────

    public async Task<OperationResult> AddAsync(InvoicePosition position)
    {
        if (position is null) return OperationResult.Fail("InvoicePosition is null");

        await using var uow = uowFactory.Create();
        await uow.InvoicePositionRepository.AddAsync(position);
        await uow.CompleteAsync();

        Positions.Add(position);
        PositionAdded?.Invoke(position);
        return OperationResult.Ok("Position added successfully");
    }

    public async Task<OperationResult> UpdateAsync(InvoicePosition position)
    {
        if (position is null) return OperationResult.Fail("InvoicePosition is null");

        await using var uow = uowFactory.Create();

        var existing = await uow.InvoicePositionRepository.GetByIdAsync(position.Id);
        if (existing is null) return OperationResult.Fail("Position not found");

        uow.InvoicePositionRepository.Update(position);
        await uow.CompleteAsync();

        var index = Positions.FindIndex(p => p.Id == position.Id);
        if (index >= 0) Positions[index] = position;
        PositionUpdated?.Invoke(position);
        return OperationResult.Ok("Position updated successfully");
    }

    public async Task<OperationResult> DeleteAsync(Guid id)
    {
        await using var uow = uowFactory.Create();
        await uow.InvoicePositionRepository.DeleteAsync(id);
        await uow.CompleteAsync();

        Positions.RemoveAll(p => p.Id == id);
        PositionDeleted?.Invoke(id);
        return OperationResult.Ok("Position deleted successfully");
    }

    public async Task<OperationResult> DeleteAllByInvoiceIdAsync(Guid invoiceId)
    {
        await using var uow = uowFactory.Create();
        await uow.InvoicePositionRepository.DeleteAllByInvoiceIdAsync(invoiceId);
        await uow.CompleteAsync();

        Positions.RemoveAll(p => p.InvoiceId == invoiceId);
        return OperationResult.Ok("All positions deleted successfully");
    }

    public async Task LoadByInvoiceIdAsync(Guid invoiceId)
    {
        await using var uow = uowFactory.Create();
        Positions = (await uow.InvoicePositionRepository.GetAllByInvoiceIdAsync(invoiceId)).ToList();
        PositionsLoaded?.Invoke();
    }

    public async Task LoadPagedByInvoiceIdAsync(Guid invoiceId, int page, int pageSize)
    {
        await using var uow = uowFactory.Create();
        CurrentPage = await uow.InvoicePositionRepository.GetPagedByInvoiceIdAsync(invoiceId, page, pageSize);
        Positions = CurrentPage.Items.ToList();
        PositionsLoaded?.Invoke();
    }

    public async Task<int> SuggestNextPositionNoAsync(Guid invoiceId)
    {
        await using var uow = uowFactory.Create();
        var all = await uow.InvoicePositionRepository.GetAllByInvoiceIdAsync(invoiceId);
        return all.Count(p => p.LineStatusReasonCode != "DETAIL") + 1;
    }

    public async Task<int> SuggestNextSubPositionNoAsync(Guid invoiceId, Guid parentId)
    {
        await using var uow = uowFactory.Create();
        var all = await uow.InvoicePositionRepository.GetAllByInvoiceIdAsync(invoiceId);
        return all.Count(p => p.ParentPositionId == parentId) + 1;
    }

    public async Task<OperationResult<Guid>> AddInvoicePositionAsync(
        Guid invoiceId, InvoicePositionDetailsDTO invPos, int? desiredPositionNo = null)
    {
        await using var uow = uowFactory.Create();

        var allPositions = (await uow.InvoicePositionRepository.GetAllByInvoiceIdAsync(invoiceId))
            .OrderBy(p => p.SortOrder).ToList();

        var topLevel = allPositions.Where(p => p.LineStatusReasonCode != "DETAIL").ToList();

        int insertIdx;
        if (desiredPositionNo.HasValue && desiredPositionNo.Value <= topLevel.Count)
        {
            var targetTopLevel = topLevel[desiredPositionNo.Value - 1];
            if (targetTopLevel.LineStatusReasonCode == "GROUP")
            {
                var firstChild = allPositions.FirstOrDefault(p => p.ParentPositionId == targetTopLevel.Id);
                insertIdx = firstChild is not null
                    ? allPositions.IndexOf(firstChild)
                    : allPositions.IndexOf(targetTopLevel);
            }
            else
            {
                insertIdx = allPositions.IndexOf(targetTopLevel);
            }
        }
        else
        {
            insertIdx = allPositions.Count;
        }

        var entity = MapToEntity(invPos, invoiceId);
        entity.Id = Guid.NewGuid();

        allPositions.Insert(insertIdx, entity);
        RebuildSortOrder(allPositions);

        await uow.InvoicePositionRepository.AddAsync(entity);
        uow.InvoicePositionRepository.UpdateRange(allPositions.Where(p => p.Id != entity.Id));
        await uow.CompleteAsync();

        Positions.Add(entity);
        PositionAdded?.Invoke(entity);

        return OperationResult<Guid>.Ok(entity.Id);
    }

    public async Task<OperationResult<Guid>> AddSubInvoicePositionAsync(
        Guid invoiceId, Guid parentId, InvoicePositionDetailsDTO subPos)
    {
        await using var uow = uowFactory.Create();

        var allPositions = (await uow.InvoicePositionRepository.GetAllByInvoiceIdAsync(invoiceId))
            .OrderBy(p => p.SortOrder).ToList();

        var parent = allPositions.FirstOrDefault(p => p.Id == parentId);
        if (parent is null)
            return OperationResult<Guid>.Fail("Parent position not found.");
        if (parent.LineStatusReasonCode != "GROUP")
            return OperationResult<Guid>.Fail("Parent is not a GROUP position.");

        int parentIdx = allPositions.IndexOf(parent);

        var entity = MapToEntity(subPos, invoiceId);
        entity.Id = Guid.NewGuid();
        entity.ParentPositionId = parentId;
        entity.LineStatusReasonCode = "DETAIL";

        allPositions.Insert(parentIdx, entity);
        RebuildSortOrder(allPositions);

        await uow.InvoicePositionRepository.AddAsync(entity);
        uow.InvoicePositionRepository.UpdateRange(allPositions.Where(p => p.Id != entity.Id));
        await uow.CompleteAsync();

        Positions.Add(entity);
        PositionAdded?.Invoke(entity);

        return OperationResult<Guid>.Ok(entity.Id);
    }

    public async Task<OperationResult<Guid>> UpdateInvoicePositionAsync(
        Guid invoiceId, Guid id, InvoicePositionDetailsDTO invPos)
    {
        await using var uow = uowFactory.Create();

        var existing = await uow.InvoicePositionRepository.GetByIdAsync(id);
        if (existing is null)
            return OperationResult<Guid>.Fail("Invoice position not found.");

        MapDtoToExistingEntity(invPos, existing);

        uow.InvoicePositionRepository.Update(existing);
        await uow.CompleteAsync();

        var index = Positions.FindIndex(p => p.Id == id);
        if (index >= 0) Positions[index] = existing;
        PositionUpdated?.Invoke(existing);

        return OperationResult<Guid>.Ok(id);
    }

    public async Task<OperationResult<Guid>> DeleteInvoicePositionAsync(Guid invoiceId, Guid id)
    {
        await using var uow = uowFactory.Create();

        var allPositions = (await uow.InvoicePositionRepository.GetAllByInvoiceIdAsync(invoiceId))
            .OrderBy(p => p.SortOrder).ToList();

        var position = allPositions.FirstOrDefault(p => p.Id == id);
        if (position is null)
            return OperationResult<Guid>.Fail("Invoice position not found.");

        var toRemoveIds = new HashSet<Guid> { id };
        if (position.LineStatusReasonCode == "GROUP")
        {
            foreach (var childId in allPositions
                .Where(p => p.ParentPositionId == id)
                .Select(p => p.Id))
                toRemoveIds.Add(childId);
        }

        var remaining = allPositions.Where(p => !toRemoveIds.Contains(p.Id)).ToList();
        RebuildSortOrder(remaining);

        if (position.LineStatusReasonCode == "GROUP")
            await uow.InvoicePositionRepository.DeleteWithChildrenAsync(id);
        else
            await uow.InvoicePositionRepository.DeleteAsync(id);

        if (remaining.Any())
            uow.InvoicePositionRepository.UpdateRange(remaining);

        await uow.CompleteAsync();

        Positions.RemoveAll(p => toRemoveIds.Contains(p.Id));
        PositionDeleted?.Invoke(id);

        return OperationResult<Guid>.Ok(id);
    }

    public async Task<OperationResult<List<InvoicePositionDetailsDTO>>> SetInvoicePositionNoAsync(
        Guid invoiceId, Guid id, int newPositionNo)
    {
        if (newPositionNo < 1) newPositionNo = 1;

        await using var uow = uowFactory.Create();

        var allPositions = (await uow.InvoicePositionRepository.GetAllByInvoiceIdAsync(invoiceId))
            .OrderBy(p => p.SortOrder).ToList();

        var position = allPositions.FirstOrDefault(p => p.Id == id);
        if (position is null)
            return OperationResult<List<InvoicePositionDetailsDTO>>.Fail("Invoice position not found.");

        if (position.LineStatusReasonCode == "DETAIL")
            return OperationResult<List<InvoicePositionDetailsDTO>>.Fail(
                "Sub-positions cannot be reordered independently.");

        var block = allPositions
            .Where(p => p.ParentPositionId == id)
            .OrderBy(p => p.SortOrder)
            .Append(position)
            .ToList();

        var blockIds = block.Select(b => b.Id).ToHashSet();
        var remaining = allPositions.Where(p => !blockIds.Contains(p.Id)).ToList();

        var topLevelRemaining = remaining.Where(p => p.LineStatusReasonCode != "DETAIL").ToList();
        int targetIndex = Math.Clamp(newPositionNo - 1, 0, topLevelRemaining.Count);

        int insertIdx;
        if (targetIndex >= topLevelRemaining.Count)
        {
            insertIdx = remaining.Count;
        }
        else
        {
            var targetTopLevel = topLevelRemaining[targetIndex];
            if (targetTopLevel.LineStatusReasonCode == "GROUP")
            {
                var firstChild = remaining.FirstOrDefault(p => p.ParentPositionId == targetTopLevel.Id);
                insertIdx = firstChild is not null
                    ? remaining.IndexOf(firstChild)
                    : remaining.IndexOf(targetTopLevel);
            }
            else
            {
                insertIdx = remaining.IndexOf(targetTopLevel);
            }
        }

        for (int i = 0; i < block.Count; i++)
            remaining.Insert(insertIdx + i, block[i]);

        RebuildSortOrder(remaining);

        uow.InvoicePositionRepository.UpdateRange(remaining);
        await uow.CompleteAsync();

        Positions = remaining;
        PositionsLoaded?.Invoke();

        return OperationResult<List<InvoicePositionDetailsDTO>>.Ok(BuildDtoList(remaining));
    }

    #region Utilities
    // Only sets SortOrder — entity has no LineId / InvoicePositionNr
    private static void RebuildSortOrder(List<InvoicePosition> orderedPositions)
    {
        for (int i = 0; i < orderedPositions.Count; i++)
            orderedPositions[i].SortOrder = i + 1;
    }

    // Computes LineId + InvoicePositionNr on the fly when building DTOs
    private static List<InvoicePositionDetailsDTO> BuildDtoList(List<InvoicePosition> orderedPositions)
    {
        // Pass 1: collect top-level LineIds
        var topLevelLineIds = new Dictionary<Guid, string>();
        int topNr = 0;
        foreach (var p in orderedPositions)
        {
            if (p.LineStatusReasonCode != "DETAIL")
                topLevelLineIds[p.Id] = (++topNr).ToString("D2");
        }

        // Pass 2: map to DTOs with correct LineId + InvoicePositionNr
        var result = new List<InvoicePositionDetailsDTO>();
        var subCountByParent = new Dictionary<Guid, int>();
        topNr = 0;

        foreach (var p in orderedPositions)
        {
            var dto = MapToDto(p);

            if (p.LineStatusReasonCode == "DETAIL" && p.ParentPositionId.HasValue)
            {
                dto.InvoicePositionNr = 0;
                var parentLineId = topLevelLineIds.TryGetValue(p.ParentPositionId.Value, out var lid) ? lid : "00";
                subCountByParent.TryAdd(p.ParentPositionId.Value, 0);
                dto.LineId = $"{parentLineId}{(++subCountByParent[p.ParentPositionId.Value]):D2}";
            }
            else
            {
                dto.InvoicePositionNr = ++topNr;
                dto.LineId = topNr.ToString("D2");
            }

            result.Add(dto);
        }

        return result;
    }

    private static InvoicePosition MapToEntity(InvoicePositionDetailsDTO dto, Guid invoiceId) => new()
    {
        Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
        InvoiceId = invoiceId,
        ParentPositionId = dto.ParentPositionId,
        LineStatusReasonCode = dto.LineStatusReasonCode,
        Description = dto.InvoicePositionDescription,
        Quantity = dto.InvoicePositionQuantity,
        UnitCode = dto.InvoicePostionUnit,
        UnitPriceNet = dto.InvoicePositionUnitPrice,
        TaxPercent = dto.InvoicePositionVatRate,
        TaxCategory = dto.InvoicePositionVatCategoryCode,
    };

    private static void MapDtoToExistingEntity(InvoicePositionDetailsDTO dto, InvoicePosition entity)
    {
        entity.Description = dto.InvoicePositionDescription;
        entity.Quantity = dto.InvoicePositionQuantity;
        entity.UnitCode = dto.InvoicePostionUnit;
        entity.UnitPriceNet = dto.InvoicePositionUnitPrice;
        entity.TaxPercent = dto.InvoicePositionVatRate;
        entity.TaxCategory = dto.InvoicePositionVatCategoryCode;
    }

    private static InvoicePositionDetailsDTO MapToDto(InvoicePosition entity) => new()
    {
        Id = entity.Id,
        ParentPositionId = entity.ParentPositionId,
        LineStatusReasonCode = entity.LineStatusReasonCode,
        InvoicePositionDescription = entity.Description,
        InvoicePositionQuantity = entity.Quantity,
        InvoicePostionUnit = entity.UnitCode,
        InvoicePositionUnitPrice = entity.UnitPriceNet,
        InvoicePositionVatRate = (int)entity.TaxPercent,
        InvoicePositionVatCategoryCode = entity.TaxCategory,
    };
    #endregion
}
