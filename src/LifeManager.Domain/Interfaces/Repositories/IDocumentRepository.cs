using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IDocumentRepository
{
    Task<List<Document>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default);
    Task<Document?> GetByIdWithAttachmentsAsync(Guid id, CancellationToken ct = default);
    Task<List<Document>> SearchAsync(Guid userId, string query, CancellationToken ct = default);
    Task AddAsync(Document document, CancellationToken ct = default);
    void Update(Document document);
    void Delete(Document document);
}
