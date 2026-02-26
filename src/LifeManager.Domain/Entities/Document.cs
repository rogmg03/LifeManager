namespace LifeManager.Domain.Entities;

public class Document : BaseEntity
{
    public Guid ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public int WordCount { get; set; }

    // Navigation
    public Project Project { get; set; } = null!;
    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}
