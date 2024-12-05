namespace Back13130
{
    public interface IParticipantRepository
    {
        Task AddParticipantAsync(Participant participant);
        Task<IEnumerable<Participant>> GetParticipantsByEventIdAsync(int eventId);
        Task<IEnumerable<Event>> GetEventsByParticipantIdAsync(int userId);
        Task RemoveParticipantAsync(int eventId, int userId);

        Task<Participant> GetUserByIdAsync(int eventId, int userId);
        Task AddParticipantToEventAsync(int eventId, int userId);
        Task RemoveParticipantToEventAsync(int eventId, int userId);

        Task Delete(int id);
    }
    public interface IEventRepository
    {
        Task<Event> GetEventByIdAsync(int id);
        Task<EventResponce> GetEventByIdAsyncPopulaetd(int id);
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task AddEventAsync(Event evnt);
        Task UpdateEventAsync(Event evnt);
        Task DeleteEventAsync(int id);
        Task<IEnumerable<Event>> GetEventsByUserIdAsync(int userId); // Optional for user-specific events
    }
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
    }

}
