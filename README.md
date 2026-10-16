# PROG_POE_Race

# A. Brief description of the system
RaceDay is an event management system designed to manage running events, categories, organisers, participants, enrolments, and race results. The system allows organisers to create and manage event categories and events, while participants can enrol in events and have their race results recorded. The system uses a SQL Server database to store and manage the information.
# B. Description of the two roles
Organiser
Creates and manages event categories.
Organises events.
Provides event details such as the date, location, distance, and entry fee.
Manages participant enrolments and race results.
Participant
Views available events.
Enrols in events.
Participates in races.
Has their finish time, position, and result status recorded.

# YouTube Link : https://youtu.be/VigStjpjXok

# PART 2 Changes
The RaceDay System is a RESTful Web API developed using ASP.NET Core Web API and C# with a Microsoft SQL Server database. The system is designed to manage sporting events, including running, walking, and cycling events.

The API supports user registration and login, user profile management, event management, event categories, participant enrolments, and race results. It uses role-based access to distinguish between Organisers and Participants.

# Technologies Used

C# – Programming language

ASP.NET Core Web API – API development framework

Microsoft SQL Server – Database management

System.Data.SqlClient – Database connectivity

SQL – Database creation, queries, and data management

Swagger – API documentation and endpoint testing

GitHub – Source code management and version control

User Roles

# 1. Organiser

Organisers are responsible for managing sporting events. Their responsibilities include:

- Registering and logging into the system.

- Viewing and updating their profile.

- Creating, viewing, updating, and deleting their own events.

- Creating categories for events.

- Viewing enrolments for events they manage.

- Recording participants' finish times and positions.

# 2. Participant

- Participants use the system to take part in sporting events. Their responsibilities include:

- Registering and logging into the system.

- Viewing and updating their profile.

- Viewing available events and categories.

- Enrolling in events and selecting categories.

- Viewing their own enrolments and race results.

# Database
The system uses a Database called Race_Day

The database contains the following tables:
Users
Organiser
Participants
Event
Category
Enrollment
Result

# 3. Configure the Connection String

The database classes use System.Data.SqlClient to connect to SQL Server.

The current LocalDB connection string is: @"Data source=(localdb)\api_guide;database=Race_Day;";

# 4. Run the API
1. Open POE_RaceDay.sln in Visual Studio.
2. Set the Web API project as the startup project.
3. Confirm that the database connection string is correct.
4. Build the solution.
5. Run the project.
