namespace TicketToCode.Api.Models;

public record EventUpdateRequest(
    string Name,
    string Description,
    int Type,
    DateTime Start,
    DateTime End,
    int MaxAttendees,
    int NumberOfTickets
); 