using System.Data.SqlClient;

namespace POE_RaceDay.Database
{
    public class result_info
    {
        private string connectionString = @"Data source=(localdb)\api_guide;database=Race_Day;";

        public string add_result(
            int enrollmentID,
            TimeSpan finishTime,
            int position)
        {
            string message =
                "Result failed to save.";

            try
            {
                using (SqlConnection connection =
                    new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO Result
                        (
                            EnrollmentID,
                            FinishTime,
                            Position,
                            ResultStatus
                        )
                        VALUES
                        (
                            @EnrollmentID,
                            @FinishTime,
                            @Position,
                            'Completed'
                        )";

                    SqlCommand command =
                        new SqlCommand(query, connection);

                    command.Parameters.AddWithValue(
                        "@EnrollmentID",
                        enrollmentID);

                    command.Parameters.AddWithValue(
                        "@FinishTime",
                        finishTime);

                    command.Parameters.AddWithValue(
                        "@Position",
                        position);

                    int rows =
                        command.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        message =
                            "Result saved successfully.";
                    }
                }
            }
            catch (Exception ex)
            {
                message =
                    "Result failed: " + ex.Message;
            }

            return message;
        }


        // =====================================================
        // PARTICIPANT VIEW OWN RESULTS
        // =====================================================

        public string get_my_results(
            int participantID)
        {
            string message = "";

            try
            {
                using (SqlConnection connection =
                    new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT
                            Result.ResultID,
                            Event.EventName,
                            Event.EventDate,
                            Category.CategoryName,
                            Result.FinishTime,
                            Result.Position,
                            Result.ResultStatus
                        FROM Result

                        INNER JOIN Enrollment
                        ON Result.EnrollmentID =
                           Enrollment.EnrollmentID

                        INNER JOIN Event
                        ON Enrollment.EventID =
                           Event.EventID

                        INNER JOIN Category
                        ON Enrollment.CategoryID =
                           Category.CategoryID

                        WHERE Enrollment.ParticipantID =
                              @ParticipantID";

                    SqlCommand command =
                        new SqlCommand(query, connection);

                    command.Parameters.AddWithValue(
                        "@ParticipantID",
                        participantID);

                    SqlDataReader reader =
                        command.ExecuteReader();

                    while (reader.Read())
                    {
                        message +=
                            "Result ID: " +
                            reader["ResultID"] +
                            " | Event: " +
                            reader["EventName"] +
                            " | Date: " +
                            reader["EventDate"] +
                            " | Category: " +
                            reader["CategoryName"] +
                            " | Finish Time: " +
                            reader["FinishTime"] +
                            " | Position: " +
                            reader["Position"] +
                            " | Status: " +
                            reader["ResultStatus"] +
                            Environment.NewLine;
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                message =
                    "Failed to get results: " +
                    ex.Message;
            }

            return message;
        }
    }
}
