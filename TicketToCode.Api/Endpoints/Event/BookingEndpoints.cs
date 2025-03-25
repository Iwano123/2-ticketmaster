using TicketToCode.Core.Data;
using TicketToCode.Core.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TicketToCode.Api.Endpoints;

public static class BookingEndpoints
{
    public static void MapBookingEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/bookings", (int eventId, int userId, int numberOfTickets, IDatabase db) =>
        {
            var success = db.BookTickets(eventId, userId, numberOfTickets);
            return success ? Results.Ok() : Results.BadRequest("Not enough tickets available");
        }).WithSummary("Book tickets for an event");

        app.MapGet("/bookings/user/{userId}", (int userId, IDatabase db) =>
        {
            var bookings = db.GetUserBookings(userId);
            return Results.Ok(bookings);
        }).WithSummary("Get bookings for a user");

        app.MapDelete("/bookings/{id}", (int id, IDatabase db) =>
        {
            var success = db.CancelBooking(id);
            return success ? Results.NoContent() : Results.NotFound();
        }).WithSummary("Cancel a booking");
    }
}