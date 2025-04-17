using TicketToCode.Core.Data;
using TicketToCode.Core.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TicketToCode.Api.Endpoints;

public static class BookingEndpoints
{
    public static void MapBookingEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/bookings", (BookingRequest request, IDatabase db) =>
        {
            var success = db.Bookings.BookTickets(request.EventId, request.UserId, request.NumberOfTickets);
            return success ? Results.Ok() : Results.BadRequest("Not enough tickets available");
        }).WithSummary("Book tickets for an event");

        app.MapGet("/bookings/user/{userId}", (int userId, IDatabase db) =>
        {
            List<Booking> bookings = db.Bookings.GetUserBookings(userId);
            return Results.Ok(bookings);
        }).WithSummary("Get bookings for a user");

        app.MapDelete("/bookings/{id}", (int id, IDatabase db) =>
        {
            bool success = db.Bookings.CancelBooking(id);
            return success ? Results.NoContent() : Results.NotFound();
        }).WithSummary("Cancel a booking");
    }
}
public record BookingRequest(int EventId, int UserId, int NumberOfTickets);
