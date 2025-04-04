using System.Net.Http.Json;
using TicketToCode.Client.Models;

namespace TicketToCode.Client.Services;

public class BookingService
{
    private readonly HttpClient _http;

    public BookingService(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> BookTickets(BookingRequest request)
    {
        var response = await _http.PostAsJsonAsync("/bookings", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<BookingDto>> GetUserBookings(int userId)
    {
        return await _http.GetFromJsonAsync<List<BookingDto>>($"/bookings/user/{userId}") ?? new();
    }

    public async Task<bool> CancelBooking(int bookingId)
    {
        var response = await _http.DeleteAsync($"/bookings/{bookingId}");
        return response.IsSuccessStatusCode;
    }
}
