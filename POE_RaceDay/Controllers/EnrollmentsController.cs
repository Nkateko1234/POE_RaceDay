using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POE_RaceDay.Database;
using System.Data.SqlClient;

namespace POE_RaceDay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private string connectionString = @"Data source=(localdb)\api_guide;database=Race_Day;";

        [HttpPost]
        public string Enroll(
            int eventID,
            int categoryID)
        {
            int? userID =
                HttpContext.Session.GetInt32("UserID");

            if (userID == null)
            {
                return "You must be logged in.";
            }

            string role =
                HttpContext.Session.GetString("Role");

            if (role != "Participant")
            {
                return
                    "Only Participants can enrol in events.";
            }

            int participantID =
                GetParticipantID(userID.Value);

            if (participantID == 0)
            {
                return "Participant account not found.";
            }

            enrollment_info enrollment =
                new enrollment_info();

            return enrollment.enroll(
                participantID,
                eventID,
                categoryID);
        }


        // =====================================================
        // PARTICIPANT VIEW OWN ENROLLMENTS
        // =====================================================

        [HttpGet("my-enrollments")]
        public string MyEnrollments()
        {
            int? userID =
                HttpContext.Session.GetInt32("UserID");

            if (userID == null)
            {
                return "You must be logged in.";
            }

            string role =
                HttpContext.Session.GetString("Role");

            if (role != "Participant")
            {
                return
                    "Only Participants can view their enrollments.";
            }

            int participantID =
                GetParticipantID(userID.Value);

            if (participantID == 0)
            {
                return "Participant account not found.";
            }

            enrollment_info enrollment =
                new enrollment_info();

            return enrollment.get_my_enrollments(
                participantID);
        }


        // =====================================================
        // ORGANISER VIEW EVENT ENROLLMENTS
        // =====================================================

        [HttpGet("event/{eventID}")]
        public string EventEnrollments(
            int eventID)
        {
            int? userID =
                HttpContext.Session.GetInt32("UserID");

            if (userID == null)
            {
                return "You must be logged in.";
            }

            string role =
                HttpContext.Session.GetString("Role");

            if (role != "Organiser")
            {
                return
                    "Only Organisers can view event enrollments.";
            }

            int organiserID =
                GetOrganiserID(userID.Value);

            if (organiserID == 0)
            {
                return "Organiser account not found.";
            }

            enrollment_info enrollment =
                new enrollment_info();

            return enrollment.get_event_enrollments(
                eventID,
                organiserID);
        }


        // =====================================================
        // GET PARTICIPANT ID
        // =====================================================

        private int GetParticipantID(int userID)
        {
            int participantID = 0;

            try
            {
                using (SqlConnection connection =
                    new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT ParticipantsID
                        FROM Participants
                        WHERE UserID = @UserID";

                    SqlCommand command =
                        new SqlCommand(query, connection);

                    command.Parameters.AddWithValue(
                        "@UserID", userID);

                    object result =
                        command.ExecuteScalar();

                    if (result != null)
                    {
                        participantID =
                            Convert.ToInt32(result);
                    }
                }
            }
            catch
            {
                participantID = 0;
            }

            return participantID;
        }


        // =====================================================
        // GET ORGANISER ID
        // =====================================================

        private int GetOrganiserID(int userID)
        {
            int organiserID = 0;

            try
            {
                using (SqlConnection connection =
                    new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT OrganiserID
                        FROM Organiser
                        WHERE UserID = @UserID";

                    SqlCommand command =
                        new SqlCommand(query, connection);

                    command.Parameters.AddWithValue(
                        "@UserID", userID);

                    object result =
                        command.ExecuteScalar();

                    if (result != null)
                    {
                        organiserID =
                            Convert.ToInt32(result);
                    }
                }
            }
            catch
            {
                organiserID = 0;
            }

            return organiserID;
        }
    }
}
