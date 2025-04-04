using System.Net.Http.Json;
using TicketToCode.Client.Models;

namespace TicketToCode.Client.Services;

public class AuthService
{
    private readonly HttpClient _http;

    public UserDto? CurrentUser { get; private set; }

    public AuthService(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> Login(string username, string password)
    {
        var response = await _http.PostAsJsonAsync("/auth/login", new LoginRequest(username, password));
        if (response.IsSuccessStatusCode)
        {
            CurrentUser = await response.Content.ReadFromJsonAsync<UserDto>();
            return true;
        }
        return false;
    }

    public async Task<bool> Register(string username, string password)
    {
        var response = await _http.PostAsJsonAsync("/auth/register", new RegisterRequest(username, password));
        return response.IsSuccessStatusCode;
    }

    public void Logout()
    {
        CurrentUser = null;
    }
}
