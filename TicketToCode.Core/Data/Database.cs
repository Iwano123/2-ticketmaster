using TicketToCode.Core.Models;

namespace TicketToCode.Core.Data;

public interface IDatabase
{
    List<Event> Events { get; set; }
    List<User> Users { get; set; }
    List<Booking> Bookings { get; set; }
    List<Event> GetUpcomingEvents();
    List<Event> SearchEvents(string query);
    Event GetEvent(int id);
    List<Booking> GetUserBookings(int userId);
    void AddEvent(Event evt);
    void UpdateEvent(Event evt);
    void DeleteEvent(int id);
    bool CancelBooking(int bookingId);
    List<Booking> GetEventBookings(int eventId);
    (int TotalEvents, int TotalBookings, int TotalUsers) GetStatistics();
    void AddUser(User user);
    User GetUser(int id);
    void DeleteUser(int id);
    void AddBooking(Booking booking);
    Booking GetBooking(int id);
    void DeleteBooking(int id);
    bool BookTickets(int eventId, int userId, int numberOfTickets);
    bool BookTicket(int eventId, int userId, int numberOfTickets);
    void UpdateUser(User updatedUser);
}

public class Database : IDatabase
{
    public List<Event> Events { get; set; } = new List<Event>();
    public List<User> Users { get; set; } = new List<User>();
    public List<Booking> Bookings { get; set; } = new List<Booking>();
    private int _eventIdCounter = 1;
    private int _userIdCounter = 1;
    private int _bookingIdCounter = 1;

    // Metoder för evenemang
    public List<Event> GetUpcomingEvents()
    {
        return Events.Where(e => e.StartTime > DateTime.Now).ToList();
    }

    public List<Event> SearchEvents(string query)
    {
        return Events.Where(e => e.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                                 e.Description.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public Event GetEvent(int id)
    {
        return Events.FirstOrDefault(e => e.Id == id);
    }

    public void AddEvent(Event evt)
    {
        evt.Id = _eventIdCounter++;
        Events.Add(evt);
    }


    public void UpdateEvent(Event updatedEvent)
    {
        var evt = Events.FirstOrDefault(e => e.Id == updatedEvent.Id);
        if (evt != null)
        {
            evt.Name = updatedEvent.Name;
            evt.Description = updatedEvent.Description;
            evt.StartTime = updatedEvent.StartTime;
            evt.EndTime = updatedEvent.EndTime;
            evt.MaxAttendees = updatedEvent.MaxAttendees;
        }
    }

    public void DeleteEvent(int id)
    {
        var evt = Events.FirstOrDefault(e => e.Id == id);
        if (evt != null)
        {
            Events.Remove(evt);
        }
    }

    // Metoder för användare
    public void AddUser(User user)
    {
        user.Id = _userIdCounter++;
        Users.Add(user);
    }

    public User GetUser(int id)
    {
        return Users.FirstOrDefault(u => u.Id == id);
    }

    public void DeleteUser(int id)
    {
        var user = Users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            Users.Remove(user);
        }
    }

    // Metoder för bokningar
    public void AddBooking(Booking booking)
    {
        booking.Id = _bookingIdCounter++;
        Bookings.Add(booking);
    }

    public Booking GetBooking(int id)
    {
        return Bookings.FirstOrDefault(b => b.Id == id);
    }

    public void DeleteBooking(int id)
    {
        var booking = Bookings.FirstOrDefault(b => b.Id == id);
        if (booking != null)
        {
            Bookings.Remove(booking);
        }
    }

    public bool BookTickets(int eventId, int userId, int numberOfTickets)
    {
        var evt = Events.FirstOrDefault(e => e.Id == eventId);
        if (evt == null || evt.MaxAttendees < numberOfTickets)
        {
            return false; // Evenemanget finns inte eller det finns inte tillräckligt med biljetter
        }

        var booking = new Booking
        {
            EventId = eventId,
            UserId = userId,
            NumberOfTickets = numberOfTickets,
            BookingDate = DateTime.Now
        };

        Bookings.Add(booking);
        evt.MaxAttendees -= numberOfTickets; // Minska antalet tillgängliga biljetter
        return true;
    }

    public List<Booking> GetUserBookings(int userId)
    {
        return Bookings.Where(b => b.UserId == userId).ToList();
    }

    public bool CancelBooking(int bookingId)
    {
        var booking = Bookings.FirstOrDefault(b => b.Id == bookingId);
        if (booking == null)
        {
            return false; // Bokningen finns inte
        }

        var evt = Events.FirstOrDefault(e => e.Id == booking.EventId);
        if (evt != null)
        {
            evt.MaxAttendees += booking.NumberOfTickets; // Återställ antalet tillgängliga biljetter
        }

        Bookings.Remove(booking);
        return true;
    }

    public List<Booking> GetEventBookings(int eventId)
    {
        return Bookings.Where(b => b.EventId == eventId).ToList();
    }

    public void UpdateUser(User updatedUser)
    {
        var user = Users.FirstOrDefault(u => u.Id == updatedUser.Id);
        if (user != null)
        {
            user.Username = updatedUser.Username;
            user.PasswordHash = updatedUser.PasswordHash;
            user.Role = updatedUser.Role;
        }
    }

    public (int TotalEvents, int TotalBookings, int TotalUsers) GetStatistics()
    {
        return (Events.Count, Bookings.Count, Users.Count);
    }
    public bool BookTicket(int eventId, int userId, int numberOfTickets)
    {
        var evt = Events.FirstOrDefault(e => e.Id == eventId);
        if (evt == null || evt.AvailableTickets < numberOfTickets)
        {
            return false; // Evenemanget finns inte eller det finns inte tillräckligt med biljetter
        }

        var booking = new Booking
        {
            EventId = eventId,
            UserId = userId,
            NumberOfTickets = numberOfTickets,
            BookingDate = DateTime.Now
        };

        Bookings.Add(booking);
        evt.AvailableTickets -= numberOfTickets; // Minska antalet tillgängliga biljetter
        return true;
    }
}