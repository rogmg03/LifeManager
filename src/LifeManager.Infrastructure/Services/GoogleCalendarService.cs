using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using LifeManager.Application.Common.Interfaces;

namespace LifeManager.Infrastructure.Services;

public class GoogleCalendarService : IGoogleCalendarService
{
    private const string ApplicationName = "LifeManager";

    public async Task<IList<Event>> GetEventsAsync(
        string calendarId,
        DateTime from,
        DateTime to,
        string accessToken,
        CancellationToken ct = default)
    {
        var service = BuildCalendarService(accessToken);

        var request = service.Events.List(calendarId);
        request.TimeMinDateTimeOffset = new DateTimeOffset(from, TimeSpan.Zero);
        request.TimeMaxDateTimeOffset = new DateTimeOffset(to, TimeSpan.Zero);
        request.SingleEvents = true;
        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
        request.MaxResults = 2500;

        var events = new List<Event>();
        string? pageToken = null;

        do
        {
            request.PageToken = pageToken;
            var page = await request.ExecuteAsync(ct);
            if (page.Items is not null)
                events.AddRange(page.Items);
            pageToken = page.NextPageToken;
        } while (pageToken is not null);

        return events;
    }

    public async Task<string> CreateEventAsync(
        string calendarId,
        Event newEvent,
        string accessToken,
        CancellationToken ct = default)
    {
        var service = BuildCalendarService(accessToken);
        var created = await service.Events.Insert(newEvent, calendarId).ExecuteAsync(ct);
        return created.Id;
    }

    public async Task DeleteEventAsync(
        string calendarId,
        string eventId,
        string accessToken,
        CancellationToken ct = default)
    {
        var service = BuildCalendarService(accessToken);
        await service.Events.Delete(calendarId, eventId).ExecuteAsync(ct);
    }

    private static CalendarService BuildCalendarService(string accessToken)
    {
        var credential = GoogleCredential
            .FromAccessToken(accessToken)
            .CreateScoped(CalendarService.Scope.Calendar);

        return new CalendarService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName
        });
    }
}
