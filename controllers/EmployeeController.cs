using Microsoft.AspNetCore.Mvc;
using EmployeeManagerAPI.Models;
using EmployeeManagerAPI.Services;
using System.Collections.Generic;

namespace EmployeeManagerAPI.Controllers
{
    // This controller handles all operations related to employees, such as fetching, creating, updating, and deleting employees.
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        // GET: api/employee/all
        // Retrieves all employees from the in-memory data store.
        [HttpGet("all")]
        public IActionResult GetAllEmployees()
        {
            var employees = InMemoryData.Employees.ToList();
            return Ok(employees); // Returns a 200 OK response with the list of employees.
        }

        // POST: api/employee/create
        // Creates a new employee record and adds it to the in-memory data store.
        [HttpPost("create")]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            // Validate the input data.
            if (employee == null || string.IsNullOrWhiteSpace(employee.FullName))
            {
                return BadRequest("Employee details are required."); // Returns a 400 Bad Request response.
            }

            // Ensure that the manager exists before assigning the employee.
            var manager = InMemoryData.Managers.FirstOrDefault(m => m.Id == employee.ManagerId);
            if (manager == null)
            {
                return BadRequest("Manager not found.");
            }

            // Assign a unique ID and set the creation date for the new employee.
            employee.Id = Guid.NewGuid(); // Generate a new GUID for the employee
            employee.CreatedDate = DateTime.UtcNow; // Set the CreatedDate to now

            // Add employee to the in-memory list
            InMemoryData.Employees.Add(employee);

            // Return a success message with the created employee's details.
            return Ok(new { message = "Employee created successfully!", employee });
        }

        // GET: api/employee/manager/{managerId}
        // Retrieves employees managed by a specific manager.
        [HttpGet("manager/{managerId}")]
        public IActionResult GetEmployeesByManager(Guid managerId)
        {
            var employees = InMemoryData.Employees.Where(e => e.ManagerId == managerId).ToList();

            if (!employees.Any())
            {
                return NotFound("No employees found for this manager."); // Returns a 404 Not Found response if no employees are found.
            }

            return Ok(employees); // Returns a 200 OK response with the list of employees.
        }

        // PUT: api/employee/update/{id}
        // Updates an existing employee's details by their ID.
        [HttpPut("update/{id}")]
        public IActionResult UpdateEmployee(Guid id, [FromBody] Employee updatedEmployee)
        {
            // Find the employee in the data store.
            var employee = InMemoryData.Employees.FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound(new { message = "Employee not found." }); // Returns a 404 Not Found response.
            }

            // Update the employee's properties.
            employee.Email = updatedEmployee.Email;
            employee.FullName = updatedEmployee.FullName;
            employee.ManagerId = updatedEmployee.ManagerId; // Optional: Only update if provided

            return Ok(new { message = "Employee updated successfully!", employee }); // Returns a 200 OK response with the updated employee.
        }

        // DELETE: api/employee/delete/{id}
        // Deletes an employee by their ID.
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            // Find the employee in the data store.
            var employee = InMemoryData.Employees.FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound(new { message = "Employee not found." }); // Returns a 404 Not Found response.
            }

            // Remove the employee from the list
            InMemoryData.Employees.Remove(employee);

            return Ok(new { message = "Employee deleted successfully!", employee }); // Returns a 200 OK response with the deleted employee's details.
        }

        // GET: api/employee/search
        // Searches employees by their name (case-insensitive) and supports auto-complete functionality.
        [HttpGet("search")]
        public IActionResult SearchEmployeesByName([FromQuery] string name)
        {
            // Validate the search query.
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name parameter is required."); // Returns a 400 Bad Request response.
            }

            // Search for employees whose names contain the provided string (case-insensitive).
            var matchingEmployees = InMemoryData.Employees
                .Where(e => e.FullName.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!matchingEmployees.Any())
            {
                return NotFound("No employees found matching the search criteria."); // Returns a 404 Not Found response if no matches are found.
            }

            return Ok(matchingEmployees); // Returns a 200 OK response with the matching employees.
        }
    }


}