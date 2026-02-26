using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class DocumentRepository : IDocumentRepository
{
    private readonly AppDbContext _context;
    public DocumentRepository(AppDbContext context) => _context = context;

    public async Task<List<Document>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default)
        => await _context.Documents
            .Where(d => d.ProjectId == projectId)
            .OrderByDescending(d => d.UpdatedAt)
            .ToListAsync(ct);

    public async Task<Document?> GetByIdWithAttachmentsAsync(Guid id, CancellationToken ct = default)
        => await _context.Documents
            .Include(d => d.Attachments)
            .FirstOrDefaultAsync(d => d.Id == id, ct);

    public async Task<List<Document>> SearchAsync(Guid userId, string query, CancellationToken ct = default)
        => await _context.Documents
            .Include(d => d.Attachments)
            .Where(d => d.Project.UserId == userId &&
                        (EF.Functions.ILike(d.Title, $"%{query}%") ||
                         (d.Content != null && EF.Functions.ILike(d.Content, $"%{query}%"))))
            .OrderByDescending(d => d.UpdatedAt)
            .ToListAsync(ct);

    public async Task AddAsync(Document document, CancellationToken ct = default)
        => await _context.Documents.AddAsync(document, ct);

    public void Update(Document document) => _context.Documents.Update(document);
    public void Delete(Document document) => _context.Documents.Remove(document);
}
