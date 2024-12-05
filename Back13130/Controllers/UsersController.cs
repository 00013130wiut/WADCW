using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;

namespace Back13130.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private JwtService jwtService;

        public UsersController(IUserRepository repo, JwtService jwt)
        {
            _userRepository = repo;
            jwtService = jwt;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(User user)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
            {
                return BadRequest("User with this email already exists.");
            }

            // Hash the password before saving it
            user.Password = HashPassword(user.Password);
            user.Role = "User"; // Default role is "User"
            await _userRepository.AddUserAsync(user);

            return Ok("User registered successfully.");
        }

        /// <summary>
        /// User login
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginRequest.Email);
            if (user == null || !VerifyPassword(loginRequest.Password, user.Password))
            {
                return Unauthorized("Invalid credentials.");
            }

            // Generate JWT token
            var token = jwtService.GenerateJwtToken(user);

            return Ok(new { Token = token, id=user.Id, name=user.Name,email=user.Email, role=user.Role });
        }

        /// <summary>
        /// Get the logged-in user's details
        /// </summary>
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return NotFound("User not found.");

            return Ok(new { user.Id, user.Name, user.Email, user.Role });
        }

        /// <summary>
        /// Get a specific user's details by Admin
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound("User not found.");

            return Ok(new { user.Id, user.Name, user.Email, user.Role });
        }

        /// <summary>
        /// Update the logged-in user's profile
        /// </summary>
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] User updatedUser)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null) return NotFound("User not found.");

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            if (!string.IsNullOrEmpty(updatedUser.Password))
            {
                user.Password = HashPassword(updatedUser.Password);
            }

            await _userRepository.UpdateUserAsync(user);
            return Ok("User profile updated successfully.");
        }

        /// <summary>
        /// Delete a user by Admin
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound("User not found.");

            await _userRepository.DeleteUserAsync(id);
            return Ok("User deleted successfully.");
        }

        /// <summary>
        /// Assign a role to a user (Admin only)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(int userId, string role)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return NotFound("User not found.");

            user.Role = role;
            await _userRepository.UpdateUserAsync(user);
            return Ok($"Role '{role}' assigned to user '{user.Name}'.");
        }

        /// <summary>
        /// List all users (Admin only)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users.Select(u => new { u.Id, u.Name, u.Email, u.Role }));
        }

        // Helper Methods

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            return HashPassword(enteredPassword) == storedPassword;
        }
    }

    public class LoginRequest
    {
        /// <example>john.doe@example.com</example>
        public string Email { get; set; }

        /// <example>password123</example>
        public string Password { get; set; }
    }
}
