﻿namespace TicketToCode.Client.Services;

public class UserState
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsAuthenticated => !string.IsNullOrEmpty(Username);
}
