using Microsoft.EntityFrameworkCore;
using System;

namespace Back13130
{
    public class EventRepository : IEventRepository
    {
        private readonly AppContext _context;

        public EventRepository(AppContext context)
        {
            _context = context;
        }



        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _context.Events.Include(e => e.Organizer).ToListAsync(); // Includes the organizer
        }

        public async Task AddEventAsync(Event evnt)
        {
            await _context.Events.AddAsync(evnt);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEventAsync(Event evnt)
        {
            _context.Events.Update(evnt);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(int id)
        {
            var evnt = await _context.Events.FindAsync(id);
            if (evnt != null)
            {
                _context.Events.Remove(evnt);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Event>> GetEventsByUserIdAsync(int userId)
        {
            return await _context.Events.Where(e => e.UserId == userId).ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _context.Events.FindAsync(id);
        }

        public async Task<EventResponce> GetEventByIdAsyncPopulaetd(int id)
        {
            return await _context.Events
                .Where(e => e.Id == id)
                .Select(e => new EventResponce
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Date = e.Date,
                    Location = e.Location,
                    Organizer = new UserResponce
                    {
                        Id = e.Organizer.Id,
                        Name = e.Organizer.Name,
                        Email = e.Organizer.Email,
                        Role = e.Organizer.Role
                    },
                    Participants = e.Participants.Select(p => new ParticipantResponce
                    {
                        EventId = p.EventId,
                        Id = p.Id,

                        User = new UserResponce
                        {
                            Id = p.User.Id,
                            Name = p.User.Name,
                            Email = p.User.Email,
                            Role = p.User.Role
                        },
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}
