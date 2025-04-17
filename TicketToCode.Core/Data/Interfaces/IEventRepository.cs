using TicketToCode.Core.Models;
using System.Linq;

namespace TicketToCode.Core.Data;

public interface IEventRepository
{
    IQueryable<Event> Events { get; }
    List<Event> GetAllEvents();
    List<Event> GetUpcomingEvents();
    List<Event> SearchEvents(string query);
    Event? GetEvent(int id);
    void AddEvent(Event evt);
    void UpdateEvent(Event evt);
    void DeleteEvent(int id);
    bool UpdateAvailableTickets(int eventId, int numberOfTickets);
} 