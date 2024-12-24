# Employee Manager API

A RESTful API for managing employee data. This project allows for the registration, search, and retrieval of employee information.

## Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/raztz15/employee-manager-api.git
   ```

2. Navigate into the project folder:

   cd employee-manager-api

3. Install the necessary dependencies:

   dotnet restore

4. Run the application:

   dotnet run

5. The server will run locally. You can access the API at http://localhost:5206

6. Why In-Memory Data was Chosen Over a Database

For this project, I opted to use in-memory data storage instead of a traditional database for the following reasons:

Development and Prototyping Speed:

The primary goal of this project is to demonstrate functionality and the ability to implement core features such as employee registration and search. Using in-memory data allows for rapid development and testing without the overhead of setting up and maintaining a database.
By skipping the database setup, I can focus on the logic and API structure, making it easier to iterate quickly and validate key functionality in the short term.
Simplified Architecture:

For small-scale applications or proof-of-concept projects like this one, in-memory data storage simplifies the architecture significantly. It removes the complexity of integrating a database and the associated configuration and connection management.
This approach is especially useful when the project doesn't require persistent data storage (e.g., for demo purposes or internal tools that don’t require long-term data retention).
No External Dependencies:

By using in-memory storage, there is no need to rely on external databases (SQL or NoSQL), which could introduce additional dependencies and potential complications with deployment and configuration.
This makes the setup process easier and more straightforward, which is beneficial in scenarios where the project is intended to be lightweight or does not yet need a fully-fledged database solution.
Focused on API Design:

The main objective was to build a robust API for managing employees. By using in-memory data, I could focus more on designing and implementing the API’s structure, including the endpoints, validation, and error handling, without getting distracted by data persistence concerns.
Scalability Considerations:

While this solution works well for a small-scale application or a demo environment, I acknowledge that in-memory storage isn’t suitable for large-scale or production applications due to its limitations in data persistence and scalability.
If the application were to be expanded for production use, I would transition to a more durable and scalable database solution (e.g., PostgreSQL, MongoDB, etc.).
In summary, in-memory data was chosen to speed up development, reduce complexity, and allow for quick iteration. However, I am aware of the limitations and would implement a more permanent database solution in a real-world scenario or for production environments.
