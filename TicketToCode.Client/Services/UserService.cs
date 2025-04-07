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

    public async Task<List<UserDto>> GetAllUsers()
    {
        return await _http.GetFromJsonAsync<List<UserDto>>("/users") ?? new();
    }

    public async Task UpdateUser(UserDto user)
    {
        await _http.PutAsJsonAsync($"/users/{user.Id}", user);
    }

    public async Task DeleteUser(int id)
    {
        await _http.DeleteAsync($"/users/{id}");
    }
}
