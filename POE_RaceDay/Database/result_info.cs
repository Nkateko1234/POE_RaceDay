using System.Data.SqlClient;
using POE_RaceDay.Models;

namespace POE_RaceDay.Database
{//start of namespace
    public class result_info
    {//start of class

        //connecton string
        private string connectionString = @"Data source=(localdb)\api_guide;database=Race_Day;";

        public string add_result( int enrollmentID, TimeSpan finishTime, int position)
        {//start of add_result method

            //message for when results fail to save 
            string message = "Result failed to save.";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
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

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@EnrollmentID", enrollmentID);

                    command.Parameters.AddWithValue("@FinishTime", finishTime);

                    command.Parameters.AddWithValue( "@Position", position);

                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        message = "Result saved successfully.";
                    }
                }
            }
            catch (Exception ex)
            {
                message = "Result failed: " + ex.Message;
            }

            return message;
        }//end of add_result method

        // PARTICIPANT VIEW OWN RESULTS
        public List<ResultModel> get_my_results(int participantID)
        {//start of get_my_result method

            // Store all results in a list
            List<ResultModel> results = new List<ResultModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
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

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue( "@ParticipantID", participantID);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ResultModel result = new ResultModel();

                        result.ResultID =Convert.ToInt32(reader["ResultID"]);

                        result.EventName = reader["EventName"].ToString() ?? "";

                        result.EventDate = Convert.ToDateTime(reader["EventDate"]);

                        result.CategoryName = reader["CategoryName"].ToString() ?? "";

                        result.FinishTime = (TimeSpan)reader["FinishTime"];

                        result.Position = Convert.ToInt32(reader["Position"]);

                        result.ResultStatus = reader["ResultStatus"].ToString() ?? "";

                        results.Add(result);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get results: " + ex.Message);
            }

            return results;
        }//end of get_my_result method

        public List<ResultModel> get_organiser_results(int organiserID)
        {
            List<ResultModel> results = new List<ResultModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT
                r.ResultID,
                r.EnrollmentID,
                p.PName AS ParticipantName,
                p.Surname,
                e.EventName,
                e.EventDate,
                c.CategoryName,
                r.FinishTime,
                r.Position,
                r.ResultStatus
            FROM Result r
            INNER JOIN Enrollment en
                ON r.EnrollmentID = en.EnrollmentID
            INNER JOIN Participants p
                ON en.ParticipantID = p.ParticipantsID
            INNER JOIN Event e
                ON en.EventID = e.EventID
            INNER JOIN Category c
                ON en.CategoryID = c.CategoryID
            WHERE e.OrganiserID = @OrganiserID
            ORDER BY e.EventDate DESC, r.Position ASC;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@OrganiserID", organiserID);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ResultModel result = new ResultModel();

                            result.ResultID = Convert.ToInt32(reader["ResultID"]);
                            result.EnrollmentID = Convert.ToInt32(reader["EnrollmentID"]);
                            result.ParticipantName = reader["ParticipantName"].ToString() ?? "";
                            result.Surname = reader["Surname"].ToString() ?? "";
                            result.EventName = reader["EventName"].ToString() ?? "";
                            result.EventDate = Convert.ToDateTime(reader["EventDate"]);
                            result.CategoryName = reader["CategoryName"].ToString() ?? "";
                            result.FinishTime = (TimeSpan)reader["FinishTime"];
                            result.Position = Convert.ToInt32(reader["Position"]);
                            result.ResultStatus = reader["ResultStatus"].ToString() ?? "";

                            results.Add(result);
                        }
                    }
                }
            }

            return results;
        }
    }//end of class
}//end of namespace
