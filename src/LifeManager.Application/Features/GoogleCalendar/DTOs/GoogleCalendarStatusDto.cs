namespace LifeManager.Application.Features.GoogleCalendar.DTOs;

public record GoogleCalendarStatusDto(
    bool IsConnected,
    string? CalendarId,
    DateTime? LastSyncedAt,
    bool IsEnabled);
