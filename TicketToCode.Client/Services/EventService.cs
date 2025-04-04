using System.Net.Http.Json;
using TicketToCode.Client.Models;

namespace TicketToCode.Client.Services;

public class EventService
{
    private readonly HttpClient _http;

    public EventService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<EventDto>> GetAllEvents()
    {
        return await _http.GetFromJsonAsync<List<EventDto>>("/events") ?? new();
    }

    public async Task<EventDto?> GetEvent(int id)
    {
        return await _http.GetFromJsonAsync<EventDto>($"/events/{id}");
    }

    public async Task<int?> CreateEvent(EventDto ev)
    {
        var response = await _http.PostAsJsonAsync("/events", ev);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<CreateResponse>();
            return result?.id;
        }
        return null;
    }

    private record CreateResponse(int id);

    

    public async Task<bool> UpdateEvent(EventDto ev)
    {
        var response = await _http.PutAsJsonAsync($"/events/{ev.Id}", ev);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteEvent(int id)
    {
        var response = await _http.DeleteAsync($"/events/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<BookingDto>> GetBookingsForEvent(int eventId)
    {
        return await _http.GetFromJsonAsync<List<BookingDto>>($"/events/{eventId}/bookings") ?? new();
    }
}
