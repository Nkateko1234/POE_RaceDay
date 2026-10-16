using System.Data.SqlClient;

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

                    SqlCommand checkCommand =  new SqlCommand(checkCategory, connection);

                    checkCommand.Parameters.AddWithValue( "@CategoryID", categoryID);

                    checkCommand.Parameters.AddWithValue( "@EventID", eventID);

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

                    SqlCommand enrollmentCheck = new SqlCommand( checkEnrollment, connection);

                    enrollmentCheck.Parameters.AddWithValue( "@ParticipantID", participantID);

                    enrollmentCheck.Parameters.AddWithValue( "@EventID", eventID);

                    int alreadyEnrolled = Convert.ToInt32( enrollmentCheck.ExecuteScalar());

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

                    SqlCommand command =  new SqlCommand(query, connection);

                    command.Parameters.AddWithValue( "@EventID", eventID);

                    command.Parameters.AddWithValue( "@CategoryID", categoryID);

                    command.Parameters.AddWithValue(  "@ParticipantID", participantID);

                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        message ="Enrollment successful.";
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
        public string get_my_enrollments( int participantID)
        {//start pf get_my_enrollments
            string message = "";

            try
            {//start of try
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

                    command.Parameters.AddWithValue( "@ParticipantID", participantID);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {//start of while
                        message +=  "Enrollment ID: " + reader["EnrollmentID"] + " | Event: " + reader["EventName"] + " | Date: " + reader["EventDate"] + " | Location: " +
                            reader["EventLocation"] +
                            " | Category: " +
                            reader["CategoryName"] +
                            " | Distance: " +
                            reader["Distance"] +
                            " | Entry Fee: " +
                            reader["EntryFee"] +
                            " | Status: " +
                            reader["EnrollmentStatus"] +
                            Environment.NewLine;
                    }//en f while

                    reader.Close();
                }
            }//end of try
            catch (Exception ex)
            {//start of catch
                message = "Failed to get enrollments: " + ex.Message;
            }//end of catch

            return message;
        }//end of get_my enrollments


        //method to get event enrollments
        public string get_event_enrollments(
            int eventID,
            int organiserID)
        {
            string message = "";

            try
            {
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
                            Category.CategoryName,
                            Category.Distance,
                            Enrollment.EnrollmentDate,
                            Enrollment.EnrollmentStatus
                        FROM Enrollment

                        INNER JOIN Participants
                        ON Enrollment.ParticipantID =
                           Participants.ParticipantsID

                        INNER JOIN Event
                        ON Enrollment.EventID =
                           Event.EventID

                        INNER JOIN Category
                        ON Enrollment.CategoryID =
                           Category.CategoryID

                        WHERE Enrollment.EventID = @EventID
                        AND Event.OrganiserID = @OrganiserID";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue( "@EventID", eventID);

                    command.Parameters.AddWithValue( "@OrganiserID", organiserID);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        message +=
                            "Enrollment ID: " +
                            reader["EnrollmentID"] +
                            " | Participant: " +
                            reader["PName"] +
                            " " +
                            reader["Surname"] +
                            " | Email: " +
                            reader["Email"] +
                            " | Event: " +
                            reader["EventName"] +
                            " | Category: " +
                            reader["CategoryName"] +
                            " | Distance: " +
                            reader["Distance"] +
                            " | Date: " +
                            reader["EnrollmentDate"] +
                            " | Status: " +
                            reader["EnrollmentStatus"] +
                            Environment.NewLine;
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                message = "Failed to get event enrollments: " + ex.Message;
            }

            return message;
        }
    }//end of class
}//end of namespace
