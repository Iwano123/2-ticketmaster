using System.Net.Http.Json;
using TicketToCode.Client.Models;
using Microsoft.JSInterop;

namespace TicketToCode.Client.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _jsRuntime;
    private const string StorageKey = "auth_user";

    public UserDto? CurrentUser { get; private set; }
    public event Action? OnAuthenticationStateChanged;

    public AuthService(HttpClient http, IJSRuntime jsRuntime)
    {
        _http = http;
        _jsRuntime = jsRuntime;
        LoadUserFromStorage();
    }

    private async void LoadUserFromStorage()
    {
        try
        {
            var storedUser = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", StorageKey);
            if (!string.IsNullOrEmpty(storedUser))
            {
                CurrentUser = System.Text.Json.JsonSerializer.Deserialize<UserDto>(storedUser);
                OnAuthenticationStateChanged?.Invoke();
            }
        }
        catch
        {
            // Ignore errors during storage access
        }
    }

    public async Task<bool> Login(string username, string password)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("/auth/login", new LoginRequest(username, password));
            if (response.IsSuccessStatusCode)
            {
                CurrentUser = await response.Content.ReadFromJsonAsync<UserDto>();
                if (CurrentUser != null)
                {
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, 
                        System.Text.Json.JsonSerializer.Serialize(CurrentUser));
                    OnAuthenticationStateChanged?.Invoke();
                }
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login error: {ex.Message}");
        }
        return false;
    }

    public async Task<bool> Register(string username, string password)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("/auth/register", new RegisterRequest(username, password));
            if (response.IsSuccessStatusCode)
            {
                CurrentUser = await response.Content.ReadFromJsonAsync<UserDto>();
                if (CurrentUser != null)
                {
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, 
                        System.Text.Json.JsonSerializer.Serialize(CurrentUser));
                    OnAuthenticationStateChanged?.Invoke();
                }
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Registration error: {ex.Message}");
        }
        return false;
    }

    public async Task Logout()
    {
        CurrentUser = null;
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", StorageKey);
        OnAuthenticationStateChanged?.Invoke();
    }
}
