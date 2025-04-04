namespace TicketToCode.Client.Models;

public class BookingDto
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int UserId { get; set; }
    public int NumberOfTickets { get; set; }
    public DateTime BookingDate { get; set; }
}
