namespace LifeManager.Application.Features.Search.DTOs;

public class SearchResultItemDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Snippet { get; set; }
    public Guid? ProjectId { get; set; }
    public string? ProjectName { get; set; }
}
