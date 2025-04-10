using TicketToCode.Core.Data;
using TicketToCode.Core.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using TicketToCode.Api.Models;

namespace TicketToCode.Api.Endpoints;

public static class EventEndpoints
{
    public static void MapEventEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPut("/events/{id}", (int id, EventUpdateRequest updatedEvent, IDatabase db) =>
        {
            var evt = db.GetEvent(id);
            if (evt == null) return Results.NotFound();

            // Map from request to database model
            var eventToUpdate = new Event
            {
                Id = id,
                Name = updatedEvent.Name,
                Description = updatedEvent.Description,
                Type = (EventType)updatedEvent.Type,
                StartTime = updatedEvent.Start,
                EndTime = updatedEvent.End,
                MaxAttendees = updatedEvent.MaxAttendees,
                AvailableTickets = updatedEvent.NumberOfTickets
            };

            db.UpdateEvent(eventToUpdate);
            return Results.NoContent();
        }).WithSummary("Update an event");

        app.MapDelete("/events/{id}", (int id, IDatabase db) =>
        {
            var evt = db.GetEvent(id);
            if (evt == null) return Results.NotFound();

            db.DeleteEvent(id);
            return Results.NoContent();
        }).WithSummary("Delete an event");
    }
}