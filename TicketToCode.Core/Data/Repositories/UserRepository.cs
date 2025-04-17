using TicketToCode.Core.Models;
using System.Linq;

namespace TicketToCode.Core.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new();
    private int _userIdCounter = 1;

    public IQueryable<User> Users => _users.AsQueryable();

    public UserRepository()
    {
        // Add default admin user
        AddUser(new User
        {
            Username = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
            Role = "Admin"
        });
    }

    public List<User> GetAllUsers() => _users;

    public User? GetUser(int id) => _users.FirstOrDefault(u => u.Id == id);

    public User? GetUserByUsername(string username) => 
        _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

    public void AddUser(User user)
    {
        user.Id = _userIdCounter++;
        _users.Add(user);
    }

    public void UpdateUser(User updatedUser)
    {
        var user = _users.FirstOrDefault(u => u.Id == updatedUser.Id);
        if (user != null)
        {
            user.Username = updatedUser.Username;
            user.PasswordHash = updatedUser.PasswordHash;
            user.Role = updatedUser.Role;
        }
    }

    public void DeleteUser(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            _users.Remove(user);
        }
    }
} 