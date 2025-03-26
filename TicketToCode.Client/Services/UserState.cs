namespace TicketToCode.Client.Services;

public class UserState
{
    public int? UserId { get; set; }
    public string? Username { get; set; }
    public string? Role { get; set; }

    public bool IsAuthenticated => !string.IsNullOrEmpty(Username);
    public bool IsAdmin => Role == "Admin";

    public void SetUser(int id, string username, string role)
    {
        UserId = id;
        Username = username;
        Role = role;
    }

    public void Clear()
    {
        UserId = null;
        Username = null;
        Role = null;
    }
}
