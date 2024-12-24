using System;

namespace EmployeeManagerAPI.Models
{
    public class Employee
    {
        // Primary Key
        public Guid Id { get; set; } = Guid.NewGuid(); // Auto-generated GUID for PK

        // Common properties
        public required string Email { get; set; }
        public required string Password { get; set; } // This will be hashed
        public required string FullName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Date of creation

        // Foreign Key
        public Guid ManagerId { get; set; } // Links to the Manager's GUID

        // Navigation Property (not necessary for in-memory, but for later database implementation)

    }
}