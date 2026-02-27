using LifeManager.Application.Features.Search.DTOs;

namespace LifeManager.Application.Common.Interfaces;

public interface ISearchReadService
{
    Task<SearchResultsDto> SearchAsync(Guid userId, string query, string? type, CancellationToken ct = default);
}
