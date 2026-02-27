namespace LifeManager.Application.Features.Search.DTOs;

public class SearchResultsDto
{
    public string Query { get; set; } = string.Empty;
    public int TotalCount { get; set; }
    public IEnumerable<SearchResultItemDto> Projects { get; set; } = [];
    public IEnumerable<SearchResultItemDto> Tasks { get; set; } = [];
    public IEnumerable<SearchResultItemDto> Documents { get; set; } = [];
    public IEnumerable<SearchResultItemDto> Activities { get; set; } = [];
    public IEnumerable<SearchResultItemDto> Clients { get; set; } = [];
}
