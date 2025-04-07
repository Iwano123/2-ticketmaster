namespace TicketToCode.Client.Models;

public class EventDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public int Type { get; set; } // EventType enum
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int MaxAttendees { get; set; }
    public int NumberOfTickets { get; set; }
}
