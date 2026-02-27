using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class CollegeCourseDetailRepository : ICollegeCourseDetailRepository
{
    private readonly AppDbContext _context;
    public CollegeCourseDetailRepository(AppDbContext context) => _context = context;

    public async Task<CollegeCourseDetail?> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default)
        => await _context.CollegeCourseDetails.FirstOrDefaultAsync(d => d.ProjectId == projectId, ct);

    public async Task AddAsync(CollegeCourseDetail detail, CancellationToken ct = default)
        => await _context.CollegeCourseDetails.AddAsync(detail, ct);

    public void Update(CollegeCourseDetail detail) => _context.CollegeCourseDetails.Update(detail);
    public void Delete(CollegeCourseDetail detail) => _context.CollegeCourseDetails.Remove(detail);
}
