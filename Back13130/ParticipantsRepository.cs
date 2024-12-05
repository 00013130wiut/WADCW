using Microsoft.EntityFrameworkCore;
using System;

namespace Back13130
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly AppContext _context;

        public ParticipantRepository(AppContext context)
        {
            _context = context;
        }

        public async Task AddParticipantAsync(Participant participant)
        {
            await _context.Participants.AddAsync(participant);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Participant>> GetParticipantsByEventIdAsync(int eventId)
        {
            return await _context.Participants
                .Include(p => p.User)
                .Where(p => p.EventId == eventId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByParticipantIdAsync(int userId)
        {
            return await _context.Participants
                .Include(p => p.Event)
                .Where(p => p.UserId == userId)
                .Select(p => p.Event)
                .ToListAsync();
        }

        public async Task RemoveParticipantAsync(int eventId, int userId)
        {
            var participant = await _context.Participants
                .FirstOrDefaultAsync(p => p.EventId == eventId && p.UserId == userId);
            if (participant != null)
            {
                _context.Participants.Remove(participant);
                await _context.SaveChangesAsync();
            }
        }

        // Get a specific participant by eventId and userId
        public async Task<Participant> GetUserByIdAsync(int eventId, int userId)
        {
            return await _context.Participants
                .FirstOrDefaultAsync(p => p.EventId == eventId && p.UserId == userId);
        }

        // Add a participant to an event (many-to-many relationship)
        public async Task AddParticipantToEventAsync(int eventId, int userId)
        {
            // Check if the user is already a participant in this event
            var existingParticipant = await GetUserByIdAsync(eventId, userId);
            if (existingParticipant != null)
            {
                throw new InvalidOperationException("User is already a participant in this event.");
            }

            var participant = new Participant
            {
                EventId = eventId,
                UserId = userId
            };

            _context.Participants.Add(participant);
            await _context.SaveChangesAsync();
        }

        // Remove a participant from an event
        public async Task RemoveParticipantToEventAsync(int eventId, int userId)
        {
            var participant = await GetUserByIdAsync(eventId, userId);
            if (participant == null)
            {
                throw new InvalidOperationException("Participant not found in this event.");
            }

            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync();
        }


        public async Task Delete(int id)
        {
            var participant = await _context.Participants.FirstOrDefaultAsync(p=>p.Id==id);
            if (participant == null)
            {
                throw new InvalidOperationException("Participant not found in this event.");
            }

            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync();
        }


    }

}
