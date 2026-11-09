using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POE_RaceDay.Database;
using System.Data.SqlClient;

namespace POE_RaceDay.Controllers
{//start of namespace
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {//start of class

        //connection string
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

        //HttpGet to view own enrollments
        [HttpGet("my-enrollments")]
        public IActionResult MyEnrollments()
        {
            int? userID =
                HttpContext.Session.GetInt32("UserID");

            if (userID == null)
            {
                return Unauthorized("You must be logged in.");
            }

            string role =
                HttpContext.Session.GetString("Role");

            if (role != "Participant")
            {
                return Forbid();
            }

            int participantID =
                GetParticipantID(userID.Value);

            if (participantID == 0)
            {
                return NotFound("Participant account not found.");
            }

            enrollment_info enrollment =new enrollment_info();

            var myEnrollments = enrollment.get_my_enrollments(participantID);

            return Ok(myEnrollments);
        }


        
        //HTTPGet for EventEnrollment
        [HttpGet("event/{eventID}")]
        public IActionResult EventEnrollments( int eventID)
        {
            int? userID = HttpContext.Session.GetInt32("UserID");

            if (userID == null)
            {
                return Unauthorized("You must be logged in.");
            }

            string role = HttpContext.Session.GetString("Role");

            if (role != "Organiser")
            {
                return Forbid();
            }

            int organiserID = GetOrganiserID(userID.Value);

            if (organiserID == 0)
            {
                return NotFound("Organiser account not found.");
            }

            enrollment_info enrollment = new enrollment_info();

            var eventEnrollments = enrollment.get_event_enrollments(eventID, organiserID);

            return Ok(eventEnrollments);
        }


        //Method to get participants
        private int GetParticipantID(int userID)
        {//start of GetParticipantID method
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
        }//end of GetParticipantID method


        private int GetOrganiserID(int userID)
        {//start of GetOrganiserID method
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
        }//end of GetOrganiserID method
    }//end of class
}//end of namespace
