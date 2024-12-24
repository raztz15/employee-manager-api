using Microsoft.AspNetCore.Mvc;
using EmployeeManagerAPI.Models;
using EmployeeManagerAPI.Services;

namespace EmployeeManagerAPI.Controllers
{
    // This controller handles operations related to managers, such as registration, login, and retrieval of all managers.
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        // POST: api/manager/register
        // Registers a new manager with validation for the required fields.
        [HttpPost("register")]
        public IActionResult RegisterManager([FromBody] Manager newManager)
        {
            // Validate input: Check if the manager object is null or required fields are missing.
            if (newManager == null || string.IsNullOrWhiteSpace(newManager.FullName))
            {
                return BadRequest("Manager details are required."); // Returns a 400 Bad Request response.
            }
            // Additional validation for full name.
            if (string.IsNullOrWhiteSpace(newManager.FullName))
            {
                return BadRequest("Full Name is required.");
            }
            // Validate password: Must be at least 6 characters long and contain at least one uppercase letter.
            if (newManager.Password.Length < 6 || !newManager.Password.Any(char.IsUpper))
            {
                return BadRequest("Password must be at least 6 characters long and contain at least one uppercase letter.");
            }
            // Validate email format.
            if (!IsValidEmail(newManager.Email))
            {
                return BadRequest("Invalid email format."); // Returns a 400 Bad Request response.
            }

            // Create a new manager with a unique ID and current creation date.
            newManager.Id = Guid.NewGuid();
            newManager.CreatedDate = DateTime.UtcNow;

            // Add the new manager to the in-memory data
            InMemoryData.Managers.Add(newManager);

            // Return a success message and the new manager's ID.
            return Ok(new { message = "Manager registered successfully!", managerId = newManager.Id });
        }
        // Helper method to validate email format using System.Net.Mail.MailAddress.
        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // POST: api/manager/login
        // Logs in a manager by verifying their email and password.
        [HttpPost("login")]
        public IActionResult LoginManager([FromBody] LoginRequest loginRequest)
        {
            // Validate input: Check if email and password are provided.
            if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return BadRequest("Email and Password are required."); // Returns a 400 Bad Request response.
            }

            // Find the manager in the in-memory data by email (case-insensitive).
            var manager = InMemoryData.Managers.FirstOrDefault(m => m.Email.Equals(loginRequest.Email, StringComparison.OrdinalIgnoreCase));

            if (manager == null)
            {
                return Unauthorized("Manager not found."); // Returns a 401 Unauthorized response.
            }

            // Check the password. In a real application, use hashed password comparison.
            if (manager.Password != loginRequest.Password)
            {
                return Unauthorized("Incorrect password."); // Returns a 401 Unauthorized response.
            }

            // Return a success message along with the manager's details.
            return Ok(new { message = "Login successful", manager });
        }

        // GET: api/manager/all
        // Retrieves all managers from the in-memory data store.
        [HttpGet("all")]
        public IActionResult GetAllManagers()
        {
            var managers = InMemoryData.Managers.ToList();
            return Ok(managers); // Returns a 200 OK response with the list of managers.
        }
    }
}