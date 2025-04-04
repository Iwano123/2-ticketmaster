using TicketToCode.Api.Endpoints;

public class GetEventBookings : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) => app
        .MapGet("/events/{id}/bookings", Handle)
        .WithSummary("Get bookings for a specific event");

    private static IResult Handle(int id, IDatabase db)
    {
        var bookings = db.GetEventBookings(id);
        return Results.Ok(bookings);
    }
}
