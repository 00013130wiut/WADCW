using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Back13130;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Back13130.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IParticipantRepository _participantRepository;

        public EventsController(IEventRepository eventRepository, IParticipantRepository participantRepository)
        {
            _eventRepository = eventRepository;
            _participantRepository = participantRepository;
        }
        /// <summary>
        /// Create a new event.
        /// </summary>
        /// <param name="event">Event details</param>
        /// <response code="201">Event created successfully</response>
        /// <response code="400">Bad request if event data is invalid</response>
        [HttpPost]
        [Authorize(Roles = "Organizer,Admin")]
        public async Task<IActionResult> CreateEvent(Event @event)
        {
            if (@event.UserId == 0)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);  // Get current user's ID from JWT token
                @event.UserId = userId;
            }
            if (@event == null)
            {
                return BadRequest("Event data is invalid.");
            }

            await _eventRepository.AddEventAsync(@event);
            return CreatedAtAction(nameof(GetEventById), new { id = @event.Id }, @event);
        }

        #region Get All Events

        /// <summary>
        /// Get all events.
        /// </summary>
        /// <response code="200">List of events</response>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _eventRepository.GetAllEventsAsync();
            return Ok(events);
        }

        #endregion

        #region Get Event By ID

        /// <summary>
        /// Get event details by ID.
        /// </summary>
        /// <param name="id">Event ID</param>
        /// <response code="200">Event found</response>
        /// <response code="404">Event not found</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEventById(int id)
        {
            var @event = await _eventRepository.GetEventByIdAsyncPopulaetd(id);
            if (@event == null)
            {
                return NotFound("Event not found.");
            }

            return Ok(@event);
        }

        #endregion


        #region Update Event

        /// <summary>
        /// Update an existing event.
        /// </summary>
        /// <param name="id">Event ID</param>
        /// <param name="event">Updated event data</param>
        /// <response code="200">Event updated successfully</response>
        /// <response code="400">Bad request if event data is invalid</response>
        /// <response code="404">Event not found</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Organizer,Admin")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] Event ev)
        {
            if (ev == null)
            {
                return BadRequest("Event data is invalid.");
            }

            var existingEvent = await _eventRepository.GetEventByIdAsync(id);
            if (existingEvent == null)
            {
                return NotFound("Event not found.");
            }

            existingEvent.Title = ev.Title;
            existingEvent.Description = ev.Description;
            existingEvent.Date = ev.Date;
            existingEvent.Location = ev.Location;

            await _eventRepository.UpdateEventAsync(existingEvent);

            return Ok(existingEvent);
        }

        #endregion

        #region Delete Event

        /// <summary>
        /// Delete an event.
        /// </summary>
        /// <param name="id">Event ID</param>
        /// <response code="200">Event deleted successfully</response>
        /// <response code="404">Event not found</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var existingEvent = await _eventRepository.GetEventByIdAsync(id);
            if (existingEvent == null)
            {
                return NotFound("Event not found.");
            }

            await _eventRepository.DeleteEventAsync(id);
            return Ok("Event deleted successfully.");
        }

        #endregion

        #region Add Participant to Event

        /// <summary>
        /// Add a participant to an event.
        /// </summary>
        /// <param name="eventId">Event ID</param>
        /// <param name="userId">User ID of the participant</param>
        /// <response code="200">Participant added successfully</response>
        /// <response code="404">Event or User not found</response>
        [HttpPost("{eventId}/participants/{userId}")]
        [Authorize(Roles = "User,Organizer,Admin")]
        public async Task<IActionResult> AddParticipantToEvent(int eventId, int userId)
        {
            var eventItem = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventItem == null)
            {
                return NotFound("Event not found.");
            }

            var user = await _participantRepository.GetUserByIdAsync(eventId,userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Add the user to the event's participants
            await _participantRepository.AddParticipantToEventAsync(eventId, userId);
            return Ok("Participant added successfully.");
        }

        #endregion

        #region Remove Participant from Event

        /// <summary>
        /// Remove a participant from an event.
        /// </summary>
        /// <param name="eventId">Event ID</param>
        /// <param name="userId">User ID of the participant</param>
        /// <response code="200">Participant removed successfully</response>
        /// <response code="404">Event or User not found</response>
        [HttpDelete("{eventId}/participants/{userId}")]
        [Authorize(Roles = "Organizer,Admin")]
        public async Task<IActionResult> RemoveParticipantFromEvent(int eventId, int userId)
        {
            var eventItem = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventItem == null)
            {
                return NotFound("Event not found.");
            }

            var user = await _participantRepository.GetUserByIdAsync(eventId,userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Remove the user from the event's participants
            await _participantRepository.RemoveParticipantToEventAsync(eventId, userId);
            return Ok("Participant removed successfully.");
        }

        #endregion
    }
}

