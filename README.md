# **Project Selection**
13139/20=656,5 so having remainder is 5. Cprsepondin project was selected 
# **Event Management System**

An **Event Management System** built using **ASP.NET Core** for the backend and **Angular 19** for the frontend. This application allows users to create, manage, and participate in events. It includes features like event creation, participant management, and drag-and-drop functionality for event rescheduling.

---

## **Features**

- **Event Management**:
  - Create, update, delete, and view events.
  - Drag-and-drop events on a calendar to reschedule them.

- **Participant Management**:
  - Add participants to events.
  - Remove participants from events.
  - View a detailed list of participants for each event.

- **User Authentication and Authorization**:
  - Secure access using JWT authentication.
  - Role-based access control (e.g., Admin, Organizer).

- **Frontend**:
  - Interactive calendar using **FullCalendar**.
  - Responsive UI with **DaisyUI** and **TailwindCSS**.

---

## **Technology Stack**

- **Backend**: ASP.NET Core 7.0
  - Entity Framework Core for database management.
  - SQL Server as the database.
- **Frontend**: Angular 19
  - FullCalendar for event display and scheduling.
  - DaisyUI and TailwindCSS for responsive design.
- **Authentication**: JSON Web Tokens (JWT).

---

## **Getting Started**

### **Prerequisites**

#### **Backend**:
- [.NET SDK](https://dotnet.microsoft.com/) (version 7.0 or later)
- SQL Server

#### **Frontend**:
- [Node.js](https://nodejs.org/) (version 16 or later)
- [Angular CLI](https://angular.io/cli)

---

### **1. Backend Setup**

#### **Step 1**: Clone the Repository
```bash
git clone https://github.com/your-repo/event-management.git
cd event-management/backend

Step 2: Configure the Database Connection
Open the appsettings.json file.
Update the connection string for SQL Server:
json
Copy code
{
  "ConnectionStrings": {
    "AppContext": "Server=YOUR_SERVER;Database=EventManagement;Trusted_Connection=True;"
  }
}
Step 3: Apply Migrations and Seed the Database
Run the following command to apply any migrations and set up the database schema:
bash
Copy code
dotnet ef database update
Step 4: Run the Backend Server
To start the backend server, use the following command:
bash
Copy code
dotnet run
The API will be available at http://localhost:5000.
2. Frontend Setup
Step 1: Navigate to the Frontend Directory
bash
Copy code
cd ../frontend
Step 2: Install Dependencies
Install the required npm dependencies for the Angular application:
bash
Copy code
npm install
Step 3: Run the Development Server
Start the Angular development server:
bash
Copy code
ng serve
The Angular app will be available at http://localhost:4200.
How to Launch Both Applications
Start the Backend API:

Navigate to the backend directory and run:
bash
Copy code
dotnet run
This will start the API at http://localhost:5000.
Start the Frontend Application:

Navigate to the frontend directory and run:
bash
Copy code
ng serve
This will start the Angular application at http://localhost:4200.
API Endpoints
Authentication
POST /api/auth/login: Logs in a user and returns a JWT.
POST /api/auth/register: Registers a new user.
Events
GET /api/events: Retrieves a list of all events.
GET /api/events/{id}: Retrieves the details of a specific event.
POST /api/events: Creates a new event.
PUT /api/events/{id}: Updates an existing event.
DELETE /api/events/{id}: Deletes an event.
Participants
POST /api/events/{eventId}/participants: Adds a participant to an event.
DELETE /api/events/{eventId}/participants/{participantId}: Removes a participant from an event.
How It Works
Users:
Register or log in to access the system.
Roles include Admin and Organizer for managing events and participants.
Events:
Users with appropriate roles can create, update, and delete events.
Events are displayed on an interactive calendar with drag-and-drop rescheduling.
Participants:
Add participants to events by entering their name and email.
View the list of participants for each event.
Remove participants from events when needed.
