using Google.Apis.Calendar.v3.Data;

namespace LifeManager.Application.Common.Interfaces;

public interface IGoogleCalendarService
{
    Task<IList<Event>> GetEventsAsync(
        string calendarId,
        DateTime from,
        DateTime to,
        string accessToken,
        CancellationToken ct = default);

    Task<string> CreateEventAsync(
        string calendarId,
        Event newEvent,
        string accessToken,
        CancellationToken ct = default);

    Task DeleteEventAsync(
        string calendarId,
        string eventId,
        string accessToken,
        CancellationToken ct = default);
}
