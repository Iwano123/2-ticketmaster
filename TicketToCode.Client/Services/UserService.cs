using System.Net.Http.Json;
using TicketToCode.Client.Models;

namespace TicketToCode.Client.Services;

public class UserService
{
    private readonly HttpClient _http;

    public UserService(HttpClient http)
    {
        _http = http;
    }

    public async Task<UserDto?> GetUser(int id)
    {
        return await _http.GetFromJsonAsync<UserDto>($"/users/{id}");
    }

    public async Task<bool> UpdateUser(UserDto user)
    {
        var response = await _http.PutAsJsonAsync($"/users/{user.Id}", user);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteUser(int id)
    {
        var response = await _http.DeleteAsync($"/users/{id}");
        return response.IsSuccessStatusCode;
    }
}
