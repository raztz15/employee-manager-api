using EmployeeManagerAPI.Models;

namespace EmployeeManagerAPI.Services
{
    public class InMemoryData
    {
        // In-memory lists to store employees and managers
        public static List<Manager> Managers = new List<Manager>();
        public static List<Employee> Employees = new List<Employee>();

        // Seed some data to simulate the in-memory DB
        static InMemoryData()
        {
            // Create Managers
            var manager1 = new Manager
            {
                Email = "manager1@example.com",
                Password = "hashedpassword1", // In real-world apps, this should be hashed
                FullName = "Manager One"
            };
            var manager2 = new Manager
            {
                Email = "manager2@example.com",
                Password = "hashedpassword2", // In real-world apps, this should be hashed
                FullName = "Manager Two"
            };

            Managers.Add(manager1);
            Managers.Add(manager2);

            // Create Employees
            var employee1 = new Employee
            {
                Email = "employee1@example.com",
                Password = "hashedpassword3", // In real-world apps, this should be hashed
                FullName = "Employee One",
                ManagerId = manager1.Id
            };

            var employee2 = new Employee
            {
                Email = "employee2@example.com",
                Password = "hashedpassword4", // In real-world apps, this should be hashed
                FullName = "Employee Two",
                ManagerId = manager1.Id
            };

            var employee3 = new Employee
            {
                Email = "employee3@example.com",
                Password = "hashedpassword5", // In real-world apps, this should be hashed
                FullName = "Employee Three",
                ManagerId = manager2.Id
            };

            Employees.Add(employee1);
            Employees.Add(employee2);
            Employees.Add(employee3);

        }
    }
}