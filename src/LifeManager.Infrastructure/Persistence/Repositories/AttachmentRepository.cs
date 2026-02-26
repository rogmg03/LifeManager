using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class AttachmentRepository : IAttachmentRepository
{
    private readonly AppDbContext _context;
    public AttachmentRepository(AppDbContext context) => _context = context;

    public async Task<Attachment?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Attachments.FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task AddAsync(Attachment attachment, CancellationToken ct = default)
        => await _context.Attachments.AddAsync(attachment, ct);

    public void Delete(Attachment attachment) => _context.Attachments.Remove(attachment);
}
