﻿using TicketToCode.Core.Models;

namespace TicketToCode.Core.Data;

public interface IDatabase
{
    IEventRepository Events { get; }
    IBookingRepository Bookings { get; }
    IUserRepository Users { get; }
    (int TotalEvents, int TotalBookings, int TotalUsers) GetStatistics();
}

public class Database : IDatabase
{
    private readonly IEventRepository _eventRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserRepository _userRepository;

    public Database()
    {
        _eventRepository = new Repositories.EventRepository();
        _bookingRepository = new Repositories.BookingRepository(_eventRepository);
        _userRepository = new Repositories.UserRepository();
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
}