using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Features.ProgressEntries.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.ProgressEntries.Commands;

public record CreateProgressEntryCommand(
    Guid GoalId,
    DateTimeOffset RecordedAt,
    decimal Value,
    string? Notes) : IRequest<ProgressEntryDto>, IBaseCommand;

public class CreateProgressEntryCommandHandler : IRequestHandler<CreateProgressEntryCommand, ProgressEntryDto>
{
    private readonly IUnitOfWork _uow;
    public CreateProgressEntryCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ProgressEntryDto> Handle(CreateProgressEntryCommand request, CancellationToken ct)
    {
        var entry = new ProgressEntry
        {
            GoalId = request.GoalId,
            RecordedAt = request.RecordedAt,
            Value = request.Value,
            Notes = request.Notes
        };

        await _uow.ProgressEntries.AddAsync(entry, ct);
        await _uow.SaveChangesAsync(ct);

        return ProgressEntryDto.FromEntity(entry);
    }
}
