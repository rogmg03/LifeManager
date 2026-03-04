using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class AssignmentDetailRepository : IAssignmentDetailRepository
{
    private readonly AppDbContext _context;
    public AssignmentDetailRepository(AppDbContext context) => _context = context;

    public async Task<AssignmentDetail?> GetByTaskIdAsync(Guid taskId, CancellationToken ct = default)
        => await _context.AssignmentDetails.FirstOrDefaultAsync(d => d.TaskId == taskId, ct);

    public async Task AddAsync(AssignmentDetail detail, CancellationToken ct = default)
        => await _context.AssignmentDetails.AddAsync(detail, ct);

    public void Update(AssignmentDetail detail) => _context.AssignmentDetails.Update(detail);
    public void Delete(AssignmentDetail detail) => _context.AssignmentDetails.Remove(detail);
}
