using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;
    public ClientRepository(AppDbContext context) => _context = context;

    public async Task<List<Client>> GetAllByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await _context.Clients.Where(c => c.UserId == userId).ToListAsync(ct);

    public async Task<Client?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Clients.FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task AddAsync(Client client, CancellationToken ct = default)
        => await _context.Clients.AddAsync(client, ct);

    public void Update(Client client) => _context.Clients.Update(client);
    public void Delete(Client client) => _context.Clients.Remove(client);
}
