using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.GoogleCalendar.Commands;

public record SyncGoogleCalendarCommand : IRequest<SyncGoogleCalendarResult>, IBaseCommand;

public record SyncGoogleCalendarResult(int Created, int Updated, int Deleted, DateTime SyncedAt);

public class SyncGoogleCalendarCommandHandler
    : IRequestHandler<SyncGoogleCalendarCommand, SyncGoogleCalendarResult>
{
    // Sync window: 7 days back, 60 days forward
    private static readonly TimeSpan SyncLookback = TimeSpan.FromDays(7);
    private static readonly TimeSpan SyncLookahead = TimeSpan.FromDays(60);

    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IGoogleCalendarService _calendarService;
    private readonly IGoogleTokenService _googleTokenService;

    public SyncGoogleCalendarCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IGoogleCalendarService calendarService,
        IGoogleTokenService googleTokenService)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _calendarService = calendarService;
        _googleTokenService = googleTokenService;
    }

    public async Task<SyncGoogleCalendarResult> Handle(
        SyncGoogleCalendarCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        // Load user and validate they have a refresh token
        var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken)
            ?? throw new InvalidOperationException("User not found.");

        if (string.IsNullOrEmpty(user.RefreshToken))
            throw new InvalidOperationException(
                "Google Calendar is not connected. Call /connect first.");

        // Load sync record
        var syncs = await _unitOfWork.GoogleCalendarSyncs
            .GetByUserIdAsync(userId, cancellationToken);

        var syncRecord = syncs.FirstOrDefault()
            ?? throw new InvalidOperationException(
                "No GoogleCalendarSync record found. Call /connect first.");

        if (!syncRecord.IsEnabled)
            throw new InvalidOperationException("Google Calendar sync is disabled.");

        // Exchange refresh token for access token
        var accessToken = await _googleTokenService.RefreshAccessTokenAsync(
            user.RefreshToken, cancellationToken);

        // Fetch events from Google
        var now = DateTime.UtcNow;
        var from = now - SyncLookback;
        var to = now + SyncLookahead;

        var events = await _calendarService.GetEventsAsync(
            syncRecord.CalendarId, from, to, accessToken, cancellationToken);

        // Load existing External blocks in the window for this user
        var existingBlocks = await _unitOfWork.ScheduleBlocks
            .GetByDateRangeAsync(userId, from, to, cancellationToken);

        var externalBlocks = existingBlocks
            .Where(b => b.BlockType == BlockType.External)
            .ToDictionary(b => b.Notes ?? string.Empty);

        int created = 0, updated = 0, deleted = 0;

        // Build set of incoming event IDs
        var incomingEventIds = new HashSet<string>();

        foreach (var ev in events)
        {
            if (string.IsNullOrEmpty(ev.Id)) continue;

            // Parse start/end (Google returns either dateTime or date)
            var start = ev.Start.DateTimeDateTimeOffset?.UtcDateTime
                ?? DateTime.SpecifyKind(
                    DateTime.Parse(ev.Start.Date),
                    DateTimeKind.Utc);

            var end = ev.End.DateTimeDateTimeOffset?.UtcDateTime
                ?? DateTime.SpecifyKind(
                    DateTime.Parse(ev.End.Date),
                    DateTimeKind.Utc);

            var title = ev.Summary ?? "(No title)";
            incomingEventIds.Add(ev.Id);

            if (externalBlocks.TryGetValue(ev.Id, out var existing))
            {
                // Update if anything changed
                if (existing.Title != title ||
                    existing.StartTime != start ||
                    existing.EndTime != end)
                {
                    existing.Title = title;
                    existing.StartTime = start;
                    existing.EndTime = end;
                    _unitOfWork.ScheduleBlocks.Update(existing);
                    updated++;
                }
            }
            else
            {
                // Create new External block; Notes stores the Google event ID
                var block = new ScheduleBlock
                {
                    UserId = userId,
                    Title = title,
                    StartTime = start,
                    EndTime = end,
                    BlockType = BlockType.External,
                    Status = BlockStatus.Scheduled,
                    Notes = ev.Id
                };
                await _unitOfWork.ScheduleBlocks.AddAsync(block, cancellationToken);
                created++;
            }
        }

        // Delete External blocks whose Google event no longer exists
        foreach (var (eventId, block) in externalBlocks)
        {
            if (!incomingEventIds.Contains(eventId))
            {
                _unitOfWork.ScheduleBlocks.Delete(block);
                deleted++;
            }
        }

        // Update LastSyncedAt
        syncRecord.LastSyncedAt = now;
        _unitOfWork.GoogleCalendarSyncs.Update(syncRecord);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new SyncGoogleCalendarResult(created, updated, deleted, now);
    }
}
