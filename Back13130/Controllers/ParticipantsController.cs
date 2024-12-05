using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Back13130.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly IParticipantRepository _participantRepository;
     

        public ParticipantsController(IParticipantRepository participantRepository, IEventRepository eventRepository)
        {
            _participantRepository = participantRepository;
            
        }

        // Get all participants for a specific event
        [HttpGet("{eventId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetParticipantsByEventId(int eventId)
        {
            var participants = await _participantRepository.GetParticipantsByEventIdAsync(eventId);

            if (!participants.Any())
            {
                return NotFound("No participants found for this event.");
            }

            // Map participants to DTO
            var result = participants.Select(p => new ParticipantResponce
            {
                Id = p.User.Id,
                Name = p.User.Name,
                Email = p.User.Email
            });

            return Ok(result);
        }


        [HttpDelete("{id}")]
       
        public async Task<IActionResult> Delete(int id)
        {
            await _participantRepository.Delete(id);

            
            return Ok();
        }



    }
    public class ParticipantResponce
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
