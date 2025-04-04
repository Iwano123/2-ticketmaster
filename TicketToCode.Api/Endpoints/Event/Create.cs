namespace TicketToCode.Api.Endpoints;
public class CreateEvent : IEndpoint
{
    // Mapping
    public static void MapEndpoint(IEndpointRouteBuilder app) => app
        .MapPost("/events", Handle)
        .WithSummary("Create event");

   
    public record Request(
        string Name,
        string Description,
        EventType Type,
        DateTime Start,
        DateTime End,
        int MaxAttendees,
        int NumberOfTickets
        );
    public record Response(int id);

    //Logic
    private static Ok<Response> Handle(Request request, IDatabase db)
    {
        var ev = new Event
        {
            Name = request.Name,
            Description = request.Description,
            Type = request.Type,
            StartTime = request.Start,
            EndTime = request.End,
            MaxAttendees = request.MaxAttendees,
            AvailableTickets = request.NumberOfTickets
        };

        db.AddEvent(ev); 

        return TypedResults.Ok(new Response(ev.Id)); 
    }

}