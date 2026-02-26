namespace LifeManager.Domain.Entities;

public class Attachment : BaseEntity
{
    public Guid? DocumentId { get; set; }
    public Guid? TaskId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string StoragePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }

    // Navigation
    public Document? Document { get; set; }
    public ProjectTask? Task { get; set; }
}
