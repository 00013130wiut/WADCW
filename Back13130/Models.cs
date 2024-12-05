namespace Back13130
{
    public class User
    {
        /// <example>1</example>
        public int Id { get; set; }

        /// <example>John Doe</example>
        public string Name { get; set; }

        /// <example>john.doe@example.com</example>
        public string Email { get; set; }

        /// <example>password123</example>
        public string Password { get; set; }

        /// <example>User</example>
        public string Role { get; set; } // Example: Admin, User, Organizer
    }

    public class UserResponce
    {
        /// <example>1</example>
        public int Id { get; set; }

        /// <example>John Doe</example>
        public string Name { get; set; }

        /// <example>john.doe@example.com</example>
        public string Email { get; set; }

      

        /// <example>User</example>
        public string Role { get; set; } // Example: Admin, User, Organizer
    }

    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public int UserId { get; set; }
        public User? Organizer { get; set; }
        public IEnumerable<Participant>? Participants { get; set; }

    }
    public class EventResponce
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public int UserId { get; set; }
        public UserResponce? Organizer { get; set; }
        public IEnumerable<ParticipantResponce>? Participants { get; set; }

    }

    public class Participant
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event? Event { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
    public class ParticipantResponce
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public EventResponce? Event { get; set; }
        public int UserId { get; set; }
        public UserResponce? User { get; set; }
    }
}
