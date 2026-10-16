Create Database RaceDay;
Use RaceDay;

create table Users
(
UserID int primary key Identity(1,1) not null,
FirstName varchar(50) not null,
LastName varchar(50) not null,
Email varchar(200) not null,
Password varchar(100) not null,
Role varchar(150) not null
);
Select * from Users;


Create table Category
(
CategoryID int primary key identity(1,1) not null,
UserID int,
CategoryName varchar(200) not null,
Distance varchar(200) not null,
EntryFee varchar(50)
CONSTRAINT FK_Category_Users
        FOREIGN KEY (UserID)
        REFERENCES Users(UserID)
);
Select * from Category;

create table Organiser
(
OrganiserID int primary key identity(1,1) not null,
CategoryID int not null,
OrganiserName varchar(100) not null,
OrganiserSurname varchar(100) not null,
Email varchar(200),
PhoneNumber varchar(20)
 CONSTRAINT FK_Organiser_Category
        FOREIGN KEY (CategoryID)
        REFERENCES Category(CategoryID)
);
SELECT * FROM Organiser;
Create table Event
(
EventID int primary key identity(1,1) not null,
CategoryID int not null,
EventType varchar(100) not null,
EventName varchar(100) not null,
EventDate varchar(100) not null,
EventLocation varchar(100) not null,
EventDecription varchar(200) not null

CONSTRAINT FK_Event_Category
        FOREIGN KEY (CategoryID)
        REFERENCES Category(CategoryID)
);
Select * from Event;
Create table Participants
(
ParticipantsID int primary key identity(1,1) not null,
PName varchar(50) not null,
Surname varchar(50) not null,
Email varchar(100),
PhoneNumber int 
);
Select * from Participants;

Create table Enrollment
(
EnrollmentID int primary key identity(1,1) not null,
EventID int,
EnrollmentDate varchar(100) not null,
EnrollmentStatus varchar(100) not null,
CONSTRAINT FK_Enrollment_Event
        FOREIGN KEY (EventID)
        REFERENCES Event(EventID)
);
Select * from Enrollment;

create Table Result
(
ResultID int primary key identity(1,1) not null,
EnrollmentID int,
ParticipantID int,
FinishTime varchar(100),
Position varchar(50),
ResultStatus varchar(100)
CONSTRAINT FK_Result_Enrollment
        FOREIGN KEY (EnrollmentID)
        REFERENCES Enrollment(EnrollmentID),

    CONSTRAINT FK_Result_Participants
        FOREIGN KEY (ParticipantID)
        REFERENCES Participants(ParticipantsID)
);

SELECT * FROM Result;