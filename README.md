

## Bus Booking System

This is a robust and scalable Bus Booking System implemented using ASP.NET Core Web API. The system caters to the needs of both administrators and travelers, offering a comprehensive set of features for efficient management of bus appointments and traveler requests. It employs JSON Web Tokens (JWT) for secure authentication and follows the Repository pattern for data access, ensuring clean separation of concerns and maintainability.

### Features:

#### Authentication and Authorization:
- **JWT Authentication**: Secure authentication mechanism using JSON Web Tokens (JWT), ensuring secure communication between clients and the server.
- **Authorization Middleware**: Implements role-based access control (RBAC) to restrict access to certain endpoints based on user roles (admin, traveler).

#### Appointment Management:
- **CRUD Operations**: Provides endpoints for administrators to perform CRUD operations on bus appointments, including adding, updating, deleting, and listing appointments.
- **Request Handling**: Allows administrators to accept or decline appointment requests made by travelers, providing a seamless booking experience.

#### Traveler Requests:
- **Appointment Viewing**: Offers endpoints for travelers to view available appointments, with the option to filter appointments based on criteria such as date, destination, etc.
- **Request Submission**: Enables travelers to submit appointment requests to the admin, specifying their preferred date, time, and destination.

#### Additional Features:
- **History Tracking**: Tracks and maintains a comprehensive history of all appointment requests made by travelers, allowing administrators to review past requests and make informed decisions.
- **Exception Handling**: Implements robust exception handling mechanisms to gracefully handle errors and provide meaningful error messages to clients.


### Technologies Used:
- **Backend**: ASP.NET Core Web API
- **Database**: Entity Framework Core with SQL Server (or your preferred database)
- **Authentication**: JSON Web Tokens (JWT)
- **Design Pattern**: Repository pattern for data access

### How to Run:
1. Clone the repository: `git clone https://github.com/yourusername/bus-booking-system.git`
2. Configure the database connection string in `appsettings.json`.
3. Run the application using Visual Studio or the .NET CLI.
4. Access the API endpoints using tools like Postman or Swagger UI.

### Contributors:
- [AMIN MOSTAFA ](https://github.com/AMIN1907)



