using System;
using System.Collections.Generic;

namespace EmployeeManagerAPI.Models
{
    public class Manager
    {
        // Primary Key
        public Guid Id { get; set; } = Guid.NewGuid(); // Auto-generated GUID for PK

        // Common properties
        public required string Email { get; set; }
        public required string Password { get; set; } // This will be hashed
        public required string FullName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Date of creation

        // List of Employees managed by the Manager
        public List<Employee> Employees { get; set; } = new List<Employee>(); // In-memory list of employees
    }
}