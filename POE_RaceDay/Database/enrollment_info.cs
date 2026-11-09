using System.Data.SqlClient;
using POE_RaceDay.Models;

namespace POE_RaceDay.Database
{//start of namespace
    public class enrollment_info
    {//start of class

        //connection string
        private string connectionString = @"Data source=(localdb)\api_guide;database=Race_Day;";

        public string enroll(int participantID, int eventID, int categoryID)
        {//start of enrollment method
            string message = "Enrollment failed.";

            try
            {//start of try
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Check that category belongs to event
                    string checkCategory = @"
                        SELECT COUNT(*)
                        FROM Category
                        WHERE CategoryID = @CategoryID
                        AND EventID = @EventID";

                    SqlCommand checkCommand = new SqlCommand(checkCategory, connection);

                    checkCommand.Parameters.AddWithValue("@CategoryID", categoryID);

                    checkCommand.Parameters.AddWithValue("@EventID", eventID);

                    int categoryExists = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (categoryExists == 0)
                    {
                        return "The selected category does not belong to this event.";
                    }


                    // Check for duplicate enrollment
                    string checkEnrollment = @"
                        SELECT COUNT(*)
                        FROM Enrollment
                        WHERE ParticipantID = @ParticipantID
                        AND EventID = @EventID";

                    SqlCommand enrollmentCheck = new SqlCommand(checkEnrollment, connection);

                    enrollmentCheck.Parameters.AddWithValue("@ParticipantID", participantID);

                    enrollmentCheck.Parameters.AddWithValue("@EventID", eventID);

                    int alreadyEnrolled = Convert.ToInt32(enrollmentCheck.ExecuteScalar());

                    if (alreadyEnrolled > 0)
                    {
                        return "Participant is already enrolled in this event.";
                    }


                    // Create enrollment
                    string query = @"
                        INSERT INTO Enrollment
                        (
                            EventID,
                            CategoryID,
                            ParticipantID,
                            EnrollmentDate,
                            EnrollmentStatus
                        )
                        VALUES
                        (
                            @EventID,
                            @CategoryID,
                            @ParticipantID,
                            GETDATE(),
                            'Enrolled'
                        )";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@EventID", eventID);

                    command.Parameters.AddWithValue("@CategoryID", categoryID);

                    command.Parameters.AddWithValue("@ParticipantID", participantID);

                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        message = "Enrollment successful.";
                    }
                }
            }//end of try
            catch (Exception ex)
            {//start of catch
                message =
                    "Enrollment failed: " +
                    ex.Message;
            }//end of catch

            return message;
        }//end of enrollment method

        //method to get enrollements
        public List<EnrollmentModel> get_my_enrollments(int participantID)
        {//start pf get_my_enrollments
            List<EnrollmentModel> enrollments = new List<EnrollmentModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                        SELECT
                            Enrollment.EnrollmentID,
                            Event.EventName,
                            Event.EventDate,
                            Event.EventLocation,
                            Category.CategoryName,
                            Category.Distance,
                            Category.EntryFee,
                            Enrollment.EnrollmentDate,
                            Enrollment.EnrollmentStatus
                        FROM Enrollment

                        INNER JOIN Event
                        ON Enrollment.EventID = Event.EventID

                        INNER JOIN Category
                        ON Enrollment.CategoryID =
                           Category.CategoryID

                        WHERE Enrollment.ParticipantID =
                              @ParticipantID";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ParticipantID", participantID);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    enrollments.Add(new EnrollmentModel
                    {
                        EnrollmentID = Convert.ToInt32(reader["EnrollmentID"]),
                        EventName = reader["EventName"].ToString() ?? "",
                        EventDate = Convert.ToDateTime(reader["EventDate"]),
                        EventLocation = reader["EventLocation"].ToString() ?? "",
                        CategoryName = reader["CategoryName"].ToString() ?? "",
                        Distance = reader["Distance"].ToString() ?? "",
                        EntryFee = Convert.ToDecimal(reader["EntryFee"]),
                        EnrollmentDate = Convert.ToDateTime(reader["EnrollmentDate"]),
                        EnrollmentStatus = reader["EnrollmentStatus"].ToString() ?? ""
                    });
                }
            }

            return enrollments;
        }


        //method to get event enrollments
        public List<EnrollmentModel> get_event_enrollments(int eventID, int organiserID)
        {
            List<EnrollmentModel> enrollments = new List<EnrollmentModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
            SELECT
                Enrollment.EnrollmentID,
                Participants.PName,
                Participants.Surname,
                Participants.Email,
                Event.EventName,
                Event.EventDate,
                Event.EventLocation,
                Category.CategoryName,
                Category.Distance,
                Category.EntryFee,
                Enrollment.EnrollmentDate,
                Enrollment.EnrollmentStatus
            FROM Enrollment
            INNER JOIN Participants
                ON Enrollment.ParticipantID = Participants.ParticipantsID
            INNER JOIN Event
                ON Enrollment.EventID = Event.EventID
            INNER JOIN Category
                ON Enrollment.CategoryID = Category.CategoryID
            WHERE Enrollment.EventID = @EventID
              AND Event.OrganiserID = @OrganiserID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EventID", eventID);
                command.Parameters.AddWithValue("@OrganiserID", organiserID);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    enrollments.Add(new EnrollmentModel
                    {
                        EnrollmentID = Convert.ToInt32(reader["EnrollmentID"]),
                        ParticipantName = reader["PName"].ToString() ?? "",
                        Surname = reader["Surname"].ToString() ?? "",
                        Email = reader["Email"].ToString() ?? "",
                        EventName = reader["EventName"].ToString() ?? "",
                        EventDate = Convert.ToDateTime(reader["EventDate"]),
                        EventLocation = reader["EventLocation"].ToString() ?? "",
                        CategoryName = reader["CategoryName"].ToString() ?? "",
                        Distance = reader["Distance"].ToString() ?? "",
                        EntryFee = Convert.ToDecimal(reader["EntryFee"]),
                        EnrollmentDate = Convert.ToDateTime(reader["EnrollmentDate"]),
                        EnrollmentStatus = reader["EnrollmentStatus"].ToString() ?? ""
                    });
                }
            }

            return enrollments;
        }
    }
 }//end of namespace
