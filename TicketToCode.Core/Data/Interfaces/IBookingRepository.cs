using TicketToCode.Core.Models;
using System.Linq;

namespace TicketToCode.Core.Data;

public interface IBookingRepository
{
    IQueryable<Booking> Bookings { get; }
    List<Booking> GetAllBookings();
    List<Booking> GetUserBookings(int userId);
    List<Booking> GetEventBookings(int eventId);
    Booking? GetBooking(int id);
    void AddBooking(Booking booking);
    void DeleteBooking(int id);
    bool CancelBooking(int bookingId);
    bool BookTickets(int eventId, int userId, int numberOfTickets);
} 