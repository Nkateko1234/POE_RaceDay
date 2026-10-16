using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POE_RaceDay.Database;
using System.Data.SqlClient;

namespace POE_RaceDay.Controllers
{//start of namespace
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {//start of class
        private string connectionString = @"Data source=(localdb)\api_guide;database=Race_Day;";

        [HttpPost]
        public string AddResult(int enrollmentID,TimeSpan finishTime,int position)
        {
            int? userID = HttpContext.Session.GetInt32("UserID");

            if (userID == null)
            {
                return "You must be logged in.";
            }

            string role = HttpContext.Session.GetString("Role");

            if (role != "Organiser")
            {
                return "Only Organisers can capture results.";
            }

            int organiserID = GetOrganiserID(userID.Value);

            if (organiserID == 0)
            {
                return "Organiser account not found.";
            }

            // Check that the enrollment belongs
            // to an event owned by this organiser
            if (!EnrollmentBelongsToOrganiser( enrollmentID, organiserID))
            {
                return "You can only capture results for your own events.";
            }

            result_info result = new result_info();

            return result.add_result( enrollmentID,finishTime, position);
        }

        // PARTICIPANT CAN VIEW THEIR OWN RESULTS
        [HttpGet("my-results")]
        public string MyResults()
        {
            int? userID = HttpContext.Session.GetInt32("UserID");

            if (userID == null)
            {
                return "You must be logged in.";
            }

            string role = HttpContext.Session.GetString("Role");

            if (role != "Participant")
            {
                return "Only Participants can view their results.";
            }

            int participantID = GetParticipantID(userID.Value);

            if (participantID == 0)
            {
                return "Participant account not found.";
            }

            result_info result = new result_info();

            return result.get_my_results(participantID);
        }

        private int GetOrganiserID(int userID)
        {
            int organiserID = 0;

            try
            {
                using (SqlConnection connection =new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT OrganiserID
                        FROM Organiser
                        WHERE UserID = @UserID";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@UserID", userID);

                    object result =command.ExecuteScalar();

                    if (result != null)
                    {
                        organiserID = Convert.ToInt32(result);
                    }
                }
            }
            catch
            {
                organiserID = 0;
            }

            return organiserID;
        }

        // GET PARTICIPANT ID
        private int GetParticipantID(int userID)
        {
            int participantID = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT ParticipantsID
                        FROM Participants
                        WHERE UserID = @UserID";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@UserID", userID);

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        participantID = Convert.ToInt32(result);
                    }
                }
            }
            catch
            {
                participantID = 0;
            }

            return participantID;
        }

        // CHECK IF ENROLLMENT BELONGS TO ORGANISER
        private bool EnrollmentBelongsToOrganiser( int enrollmentID, int organiserID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT COUNT(*)
                        FROM Enrollment

                        INNER JOIN Event
                        ON Enrollment.EventID =
                           Event.EventID

                        WHERE Enrollment.EnrollmentID =
                              @EnrollmentID

                        AND Event.OrganiserID =
                            @OrganiserID";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@EnrollmentID",enrollmentID);

                    command.Parameters.AddWithValue( "@OrganiserID",organiserID);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }//end of class
}//end of namespace
