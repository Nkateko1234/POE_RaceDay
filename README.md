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

# Youtube Link
Youtube: https://youtu.be/NsEi9I6HNKw

# Part 3 changes

# 1. Project Overview

RaceDay is a web-based race and event management system developed using ASP.NET Core. The system allows Organisers to manage running events, race categories, participant enrolments and race results. Participants can register, log in, browse available events, enrol in races and view their results.

Part 3 extends the RESTful API developed in Part 2 by introducing an ASP.NET Core MVC front end. The MVC application communicates with the API to perform data operations instead of accessing the database directly.

The application aims to provide a user-friendly, responsive and secure platform for managing race events.

# 2. Project Objectives

The main objectives of Part 3 are to:

Develop an ASP.NET Core MVC front end.
Consume the RESTful API developed in Part 2.
Implement session-based authentication.
Provide different interfaces for Organisers and Participants.
Allow Organisers to manage events, categories, enrolments and results.
Allow Participants to browse events, enrol and view results.
Integrate Azure Blob Storage for image uploads through the API.
Containerise the ASP.NET Core application using Docker.

Maintain separation between the MVC front end, API and database.

# 3. Technologies Used
C# - Application programming language
ASP.NET Core MVC- Front-end web application
ASP.NET Core Web API- Backend REST API
SQL Server - Database management
ADO.NET / SQL Client - Database operations in the API
HTML5 - Web page structure
CSS3 - Website styling
Bootstrap - Responsive user interface
JavaScript - Client-side functionality
Visual Studio - Development environment
Swagger - API testing and documentation

# 4. User Roles
#The system supports two user roles.

## Organiser

Organisers are responsible for managing race events.
Organiser functionality includes:
Registering and logging into the system.
Accessing the Organiser Dashboard.
Creating events.
Viewing events.
Updating event information.
Deleting events.
Creating race categories.
Viewing participant enrolments.
Capturing race results.


## Participant

Participants use the system to discover and participate in races.
Participant functionality includes:
Registering and logging into the system.
Accessing the Participant Dashboard.
Browsing available events.
Viewing event details.
Enrolling in events and categories.
Viewing personal enrolments.
Viewing race results.

Role-based access prevents users from accessing functionality that does not belong to their role.

# 5. MVC Project Structure

The MVC application is organised into the following folders:

POE_RaceDay_MVC
│
├── Controllers
│   ├── AuthController.cs
│   ├── HomeController.cs
│   ├── OrganiserController.cs
│   └── ParticipantController.cs
│
├── Models
│   ├── LoginViewModel.cs
│   ├── RegisterViewModel.cs
│   ├── EventViewModel.cs
│   ├── CategoryViewModel.cs
│   ├── EnrollmentViewModel.cs
│   ├── ResultViewModel.cs
│   └── CaptureResultViewModel.cs
│
├── Views
│   ├── Auth
│   ├── Home
│   ├── Organiser
│   ├── Participant
│   └── Shared
│
├── wwwroot
│   ├── css
│   │   └── site.css
│   ├── images
│   └── lib
│
├── appsettings.json
├── Program.cs
└── README.md

The API is maintained separately in the POE_RaceDay project.

6. MVC Project Structure

The MVC application is organised into the following folders:

POE_RaceDay_MVC
│
├── Controllers
│   ├── AuthController.cs
│   ├── HomeController.cs
│   ├── OrganiserController.cs
│   └── ParticipantController.cs
│
├── Models
│   ├── LoginViewModel.cs
│   ├── RegisterViewModel.cs
│   ├── EventViewModel.cs
│   ├── CategoryViewModel.cs
│   ├── EnrollmentViewModel.cs
│   ├── ResultViewModel.cs
│   └── CaptureResultViewModel.cs
│
├── Views
│   ├── Auth
│   ├── Home
│   ├── Organiser
│   ├── Participant
│   └── Shared
│
├── wwwroot
│   ├── css
│   │   └── site.css
│   ├── images
│   └── lib
│
├── appsettings.json
├── Program.cs
└── README.md

The API is maintained separately in the POE_RaceDay project.

# 6. Main MVC Features
## 6.1 Home Page
The homepage introduces the RaceDay system and provides navigation to registration and login.

It includes:

RaceDay branding and logo.
Homepage promotional content.
Registration and login buttons.
Discover, Compete and Achieve feature cards.
Responsive styling.

## 6.2 Authentication

The authentication functionality allows users to register and log in.
The MVC application sends authentication requests to the API.

After a successful login, the MVC application stores relevant user information in its session, including:

UserID
Role
API session cookie
The API session cookie is forwarded when authenticated requests are made to protected API endpoints.

## 6.3 Organiser Dashboard

The Organiser Dashboard provides access to event and race management functionality.

Available management areas include:
Event Management
Category Management
Event Enrolments
Race Results

## 6.4 Participant Dashboard

The Participant Dashboard provides access to participant functionality.

Available sections include:
Browse Events
My Enrolments
My Results

## 6.5 Event Management

Organisers can:
Create new events.
View existing events.
Edit event information.
Delete events.
Event information includes the event name, type, date, location and description.

## 6.6 Category Management

Organisers can create race categories for their events.

Category information includes:
Category name.
Distance.
Entry fee.
Associated event.

## 6.7 Participant Enrolments

Participants can enrol in available race categories.

Organisers can view participants enrolled in their events.

The system validates enrolment requests through the API.

## 6.8 Race Results

Organisers can capture participant race results by entering:

Enrolment ID.
Finish time.
Finishing position.
Participants can view their recorded results through their dashboard.

## 7 Test the Application

1.Register an Organiser account.
2.Log in as an Organiser.
3.Create an event.
4.Create categories for the event.
5.Register a Participant account.
6.Log in as a Participant.
7.Browse the available events.
8.Enrol in an event.
9.Log in as the Organiser and view the enrolment.
10.Capture a race result.
11.Log in as the Participant and view the result.

