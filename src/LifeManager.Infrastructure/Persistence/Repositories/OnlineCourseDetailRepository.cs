using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class OnlineCourseDetailRepository : IOnlineCourseDetailRepository
{
    private readonly AppDbContext _context;
    public OnlineCourseDetailRepository(AppDbContext context) => _context = context;

    public async Task<OnlineCourseDetail?> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default)
        => await _context.OnlineCourseDetails.FirstOrDefaultAsync(d => d.ProjectId == projectId, ct);

    public async Task AddAsync(OnlineCourseDetail detail, CancellationToken ct = default)
        => await _context.OnlineCourseDetails.AddAsync(detail, ct);

    public void Update(OnlineCourseDetail detail) => _context.OnlineCourseDetails.Update(detail);
    public void Delete(OnlineCourseDetail detail) => _context.OnlineCourseDetails.Remove(detail);
}
