using TicketToCode.Core.Models;

namespace TicketToCode.Core.Data;

public class UnitOfWork : IDisposable
{
    private readonly IEventRepository _eventRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserRepository _userRepository;

    public UnitOfWork(
        IEventRepository eventRepository,
        IBookingRepository bookingRepository,
        IUserRepository userRepository)
    {
        _eventRepository = eventRepository;
        _bookingRepository = bookingRepository;
        _userRepository = userRepository;
    }

    public IEventRepository Events => _eventRepository;
    public IBookingRepository Bookings => _bookingRepository;
    public IUserRepository Users => _userRepository;

    public (int TotalEvents, int TotalBookings, int TotalUsers) GetStatistics()
    {
        return (
            _eventRepository.GetAllEvents().Count,
            _bookingRepository.GetAllBookings().Count,
            _userRepository.GetAllUsers().Count
        );
    }

    public void Dispose()
    {
        // Clean up resources if needed
    }
} 