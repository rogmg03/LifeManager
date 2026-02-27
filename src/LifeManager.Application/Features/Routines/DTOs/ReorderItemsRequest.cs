namespace LifeManager.Application.Features.Routines.DTOs;

public record ReorderItemsRequest(IReadOnlyList<ReorderItemEntry> Items);

public record ReorderItemEntry(Guid Id, int SortOrder);
