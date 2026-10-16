Create Database Race_Day;
Use Race_Day;
CREATE TABLE Users
(
    UserID INT IDENTITY(1,1) PRIMARY KEY,

    FirstName VARCHAR(50) NOT NULL,

    LastName VARCHAR(50) NOT NULL,

    Email VARCHAR(200) NOT NULL UNIQUE,

    Password VARCHAR(255) NOT NULL,

    Role VARCHAR(20) NOT NULL,

    CONSTRAINT CK_Users_Role
        CHECK (Role IN ('Organiser', 'Participant'))
);

CREATE TABLE Organiser
(
    OrganiserID INT IDENTITY(1,1) PRIMARY KEY,

    UserID INT NOT NULL UNIQUE,

    OrganiserName VARCHAR(100) NOT NULL,

    OrganiserSurname VARCHAR(100) NOT NULL,

    Email VARCHAR(200),

    PhoneNumber VARCHAR(20),

    CONSTRAINT FK_Organiser_Users
        FOREIGN KEY (UserID)
        REFERENCES Users(UserID)
);


CREATE TABLE Participants
(
    ParticipantsID INT IDENTITY(1,1) PRIMARY KEY,

    UserID INT NOT NULL UNIQUE,

    PName VARCHAR(50) NOT NULL,

    Surname VARCHAR(50) NOT NULL,

    Email VARCHAR(200),

    PhoneNumber VARCHAR(20),

    CONSTRAINT FK_Participants_Users
        FOREIGN KEY (UserID)
        REFERENCES Users(UserID)
);

CREATE TABLE Event
(
    EventID INT IDENTITY(1,1) PRIMARY KEY,

    OrganiserID INT NOT NULL,

    EventType VARCHAR(20) NOT NULL,

    EventName VARCHAR(100) NOT NULL,

    EventDate DATE NOT NULL,

    EventLocation VARCHAR(100) NOT NULL,

    EventDescription VARCHAR(200) NOT NULL,

    CONSTRAINT FK_Event_Organiser
        FOREIGN KEY (OrganiserID)
        REFERENCES Organiser(OrganiserID),

    CONSTRAINT CK_Event_Type
        CHECK (EventType IN ('Run', 'Walk', 'Cycle'))
);


CREATE TABLE Category
(
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,

    EventID INT NOT NULL,

    CategoryName VARCHAR(200) NOT NULL,

    Distance VARCHAR(50) NOT NULL,

    EntryFee DECIMAL(10,2),

    CONSTRAINT FK_Category_Event
        FOREIGN KEY (EventID)
        REFERENCES Event(EventID)
);

CREATE TABLE Enrollment
(
    EnrollmentID INT IDENTITY(1,1) PRIMARY KEY,

    EventID INT NOT NULL,

    CategoryID INT NOT NULL,

    ParticipantID INT NOT NULL,

    EnrollmentDate DATE NOT NULL
        DEFAULT GETDATE(),

    EnrollmentStatus VARCHAR(50) NOT NULL
        DEFAULT 'Enrolled',

    CONSTRAINT FK_Enrollment_Event
        FOREIGN KEY (EventID)
        REFERENCES Event(EventID),

    CONSTRAINT FK_Enrollment_Category
        FOREIGN KEY (CategoryID)
        REFERENCES Category(CategoryID),

    CONSTRAINT FK_Enrollment_Participant
        FOREIGN KEY (ParticipantID)
        REFERENCES Participants(ParticipantsID)
);

CREATE TABLE Result
(
    ResultID INT IDENTITY(1,1) PRIMARY KEY,

    EnrollmentID INT NOT NULL UNIQUE,

    FinishTime TIME,

    Position INT,

    ResultStatus VARCHAR(50) NOT NULL
        DEFAULT 'Completed',

    CONSTRAINT FK_Result_Enrollment
        FOREIGN KEY (EnrollmentID)
        REFERENCES Enrollment(EnrollmentID)
);

INSERT INTO Users
    (FirstName, LastName, Email, Password, Role)
VALUES
    ('Nkateko', 'Manganyi', 'nkateko10@gmail.com', 'Nkateko123', 'Organiser'),
    ('Siyanda', 'Ndlovu', 'siyanda1632@gmail.com', 'siyandlovu1634', 'Organiser'),
    ('Thabo', 'Jay', 'thabojj99@gmail.com', 'ThaboJJay@776', 'Participant'),
    ('Mpho', 'Padime', 'MphoPhopho@gmail.com', 'Phopho_Pa@1234', 'Participant');

INSERT INTO Organiser
    (UserID, OrganiserName, OrganiserSurname, Email, PhoneNumber)
VALUES
    (1, 'Nkateko', 'Manganyi', 'nkateko10@gmail.com', '0712345678'),
    (2, 'Siyanda', 'Ndlovu', 'siyanda1632@gmail.com', '0723456789');


INSERT INTO Participants
    (UserID, PName, Surname, Email, PhoneNumber)
VALUES
    (3, 'Thabo', 'Jay', 'thabojj99@gmail.com', '0734567890'),
    (4, 'Mpho', 'Padime', 'MphoPhopho@gmail.com', '0745678901');

INSERT INTO Event
    (OrganiserID, EventType, EventName, EventDate,
     EventLocation, EventDescription)
VALUES
    (1, 'Run', 'Cape Town City Run',
     '2026-11-15',
     'Cape Town Stadium',
     'A city running event open to runners of different age groups.'),

    (1, 'Walk', 'Johannesburg Charity Walk',
     '2026-12-05',
     'Zoo Lake',
     'A community charity walk supporting local development projects.'),

    (2, 'Cycle', 'Pretoria Cycle Challenge',
     '2027-01-20',
     'Union Buildings',
     'A cycling challenge for beginner and experienced cyclists.');


INSERT INTO Category
    (EventID, CategoryName, Distance, EntryFee)
VALUES
    (1, 'Junior Run', '5 KM', 80.00),
    (1, 'Open Run', '10 KM', 120.00),
    (1, 'Professional Run', '21 KM', 200.00),

    (2, 'Family Walk', '5 KM', 50.00),
    (2, 'Open Walk', '10 KM', 80.00),

    (3, 'Beginner Cycle', '20 KM', 150.00),
    (3, 'Advanced Cycle', '50 KM', 250.00);

INSERT INTO Enrollment
    (EventID, CategoryID, ParticipantID,
     EnrollmentDate, EnrollmentStatus)
VALUES
   
    (1, 2, 1, '2026-10-20', 'Enrolled'),
    (1, 1, 2, '2026-10-21', 'Enrolled'),
    (2, 4, 1, '2026-10-22', 'Enrolled'),
    (3, 6, 2, '2026-10-23', 'Enrolled');

SELECT * FROM Users;
SELECT * FROM Organiser;
SELECT * FROM Participants;
SELECT * FROM Event;
SELECT * FROM Category;
SELECT * FROM Enrollment;