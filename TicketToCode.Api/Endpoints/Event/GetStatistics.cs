using TicketToCode.Api.Endpoints;

public class GetStatistics : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) => app
        .MapGet("/statistics", Handle)
        .WithSummary("Get overall statistics");

    public record Response(int TotalEvents, int TotalBookings, int TotalUsers);

    private static IResult Handle(IDatabase db)
    {
        var stats = db.GetStatistics();
        return Results.Ok(new Response(stats.TotalEvents, stats.TotalBookings, stats.TotalUsers));
    }
}
