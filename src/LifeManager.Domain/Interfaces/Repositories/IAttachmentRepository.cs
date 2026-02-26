using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IAttachmentRepository
{
    Task<Attachment?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Attachment attachment, CancellationToken ct = default);
    void Delete(Attachment attachment);
}
