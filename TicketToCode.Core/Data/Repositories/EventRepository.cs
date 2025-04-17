using TicketToCode.Core.Models;
using System.Linq;

namespace TicketToCode.Core.Data.Repositories;

public class EventRepository : IEventRepository
{
    private readonly List<Event> _events = new();
    private int _eventIdCounter = 1;

    public IQueryable<Event> Events => _events.AsQueryable();

    public List<Event> GetAllEvents() => _events;

    public List<Event> GetUpcomingEvents() => 
        _events.Where(e => e.StartTime > DateTime.Now).ToList();

    public List<Event> SearchEvents(string query) =>
        _events.Where(e => e.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                          e.Description.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();

    public Event? GetEvent(int id) => _events.FirstOrDefault(e => e.Id == id);

    public void AddEvent(Event evt)
    {
        evt.Id = _eventIdCounter++;
        _events.Add(evt);
    }

    public void UpdateEvent(Event updatedEvent)
    {
        var evt = _events.FirstOrDefault(e => e.Id == updatedEvent.Id);
        if (evt != null)
        {
            evt.Name = updatedEvent.Name;
            evt.Description = updatedEvent.Description;
            evt.Type = updatedEvent.Type;
            evt.StartTime = updatedEvent.StartTime;
            evt.EndTime = updatedEvent.EndTime;
            evt.MaxAttendees = updatedEvent.MaxAttendees;
            evt.AvailableTickets = updatedEvent.AvailableTickets;
        }
    }

    public void DeleteEvent(int id)
    {
        var evt = _events.FirstOrDefault(e => e.Id == id);
        if (evt != null)
        {
            _events.Remove(evt);
        }
    }

    public bool UpdateAvailableTickets(int eventId, int numberOfTickets)
    {
        var evt = _events.FirstOrDefault(e => e.Id == eventId);
        if (evt == null) return false;

        evt.AvailableTickets = numberOfTickets;
        return true;
    }
} 