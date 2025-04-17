using TicketToCode.Core.Data;
using TicketToCode.Core.Models;

namespace TicketToCode.Api.Services;

public interface IAuthService
{
    UserDto? Login(string username, string password);
    UserDto? Register(string username, string password);
}

public class AuthService : IAuthService
{
    private readonly IDatabase _database;

    public AuthService(IDatabase database)
    {
        _database = database;
    }

    public UserDto? Login(string username, string password)
    {
        Console.WriteLine($"Attempting login for user: {username}");
        var allUsers = _database.Users.GetAllUsers();
        Console.WriteLine($"Total users in database: {allUsers.Count}");
        Console.WriteLine($"Users in database: {string.Join(", ", allUsers.Select(u => $"{u.Username}({u.Role})"))}");

        var user = _database.Users.GetUserByUsername(username);
            
        if (user == null)
        {
            Console.WriteLine($"User {username} not found in database");
            return null;
        }

        Console.WriteLine($"Found user: {user.Username} with role: {user.Role}");
        
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            Console.WriteLine($"Password verification failed for user {username}");
            return null;
        }

        Console.WriteLine($"Login successful for user {username} with role {user.Role}");
        return new UserDto(user.Id, user.Username, user.Role);
    }

    public UserDto? Register(string username, string password)
    {
        var existingUser = _database.Users.GetUserByUsername(username);
        if (existingUser != null)
        {
            Console.WriteLine($"Registration failed - username {username} already exists");
            return null;
        }

        var hashedPwd = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User(username, hashedPwd);
        _database.Users.AddUser(user);

        Console.WriteLine($"New user registered: {username} with role {user.Role}");
        return new UserDto(user.Id, user.Username, user.Role);
    }
}