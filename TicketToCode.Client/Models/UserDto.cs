namespace TicketToCode.Client.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Role { get; set; } = "User";

        public UserDto() { } // ← behövs för tom initiering

        public UserDto(int id, string username, string role)
        {
            Id = id;
            Username = username;
            Role = role;
        }
    }
}
