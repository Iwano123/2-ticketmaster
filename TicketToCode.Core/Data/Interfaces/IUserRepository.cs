using TicketToCode.Core.Models;
using System.Linq;

namespace TicketToCode.Core.Data;

public interface IUserRepository
{
    IQueryable<User> Users { get; }
    List<User> GetAllUsers();
    User? GetUser(int id);
    User? GetUserByUsername(string username);
    void AddUser(User user);
    void UpdateUser(User user);
    void DeleteUser(int id);
} 