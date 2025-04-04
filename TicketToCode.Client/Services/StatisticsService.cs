using System.Net.Http.Json;
using TicketToCode.Client.Models;

namespace TicketToCode.Client.Services;

public class StatisticsService
{
    private readonly HttpClient _http;

    public StatisticsService(HttpClient http)
    {
        _http = http;
    }

    public async Task<StatisticsDto?> GetStats()
    {
        return await _http.GetFromJsonAsync<StatisticsDto>("/statistics");
    }
}
