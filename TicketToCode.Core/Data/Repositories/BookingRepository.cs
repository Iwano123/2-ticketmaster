using TicketToCode.Core.Models;
using System.Linq;

namespace TicketToCode.Core.Data.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly List<Booking> _bookings = new();
    private readonly IEventRepository _eventRepository;
    private int _bookingIdCounter = 1;

    public IQueryable<Booking> Bookings => _bookings.AsQueryable();

    public BookingRepository(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public List<Booking> GetAllBookings() => _bookings;

    public List<Booking> GetUserBookings(int userId) => 
        _bookings.Where(b => b.UserId == userId).ToList();

    public List<Booking> GetEventBookings(int eventId) => 
        _bookings.Where(b => b.EventId == eventId).ToList();

    public Booking? GetBooking(int id) => _bookings.FirstOrDefault(b => b.Id == id);

    public void AddBooking(Booking booking)
    {
        booking.Id = _bookingIdCounter++;
        _bookings.Add(booking);
    }

    public void DeleteBooking(int id)
    {
        var booking = _bookings.FirstOrDefault(b => b.Id == id);
        if (booking != null)
        {
            _bookings.Remove(booking);
        }
    }

    public bool CancelBooking(int bookingId)
    {
        var booking = _bookings.FirstOrDefault(b => b.Id == bookingId);
        if (booking == null) return false;

        var evt = _eventRepository.GetEvent(booking.EventId);
        if (evt != null)
        {
            evt.AvailableTickets += booking.NumberOfTickets;
        }

        _bookings.Remove(booking);
        return true;
    }

    public bool BookTickets(int eventId, int userId, int numberOfTickets)
    {
        var evt = _eventRepository.GetEvent(eventId);
        if (evt == null || evt.AvailableTickets < numberOfTickets)
        {
            return false;
        }

        var booking = new Booking
        {
            EventId = eventId,
            UserId = userId,
            NumberOfTickets = numberOfTickets,
            BookingDate = DateTime.Now
        };

        AddBooking(booking);
        evt.AvailableTickets -= numberOfTickets;
        return true;
    }
} 