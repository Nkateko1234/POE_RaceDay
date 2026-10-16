using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POE_RaceDay.Database;
using System.Data.SqlClient;

namespace POE_RaceDay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        [HttpGet]
        public string GetEvents()
        {
            event_info events = new event_info();

            return events.get_events();
        }


        // GET ONE EVENT
        [HttpGet("{eventID}")]
        public string GetEvent(int eventID)
        {
            event_info events = new event_info();

            return events.get_event(eventID);
        }

        // CREATE EVENT
        [HttpPost]
        public string CreateEvent(
            string eventType,
            string eventName,
            DateTime eventDate,
            string eventLocation,
            string eventDescription)
        {
            // Check whether the user is logged in
            int? userID =
                HttpContext.Session.GetInt32("UserID");

            if (userID == null)
            {
                return "You must be logged in.";
            }

            // Check role
            string role =
                HttpContext.Session.GetString("Role");

            if (role != "Organiser")
            {
                return "Only Organisers can create events.";
            }

            // Get OrganiserID
            int organiserID =
                GetOrganiserID(userID.Value);

            if (organiserID == 0)
            {
                return "Organiser account not found.";
            }

            event_info events = new event_info();

            return events.create_event(
                organiserID,
                eventType,
                eventName,
                eventDate,
                eventLocation,
                eventDescription);
        }

        // UPDATE EVENT for Organisers only
        [HttpPut("{eventID}")]
        public string UpdateEvent(
            int eventID,
            string eventType,
            string eventName,
            DateTime eventDate,
            string eventLocation,
            string eventDescription)
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
                return "Only Organisers can update events.";
            }

            int organiserID =
                GetOrganiserID(userID.Value);

            if (organiserID == 0)
            {
                return "Organiser account not found.";
            }

            event_info events = new event_info();

            return events.update_event(
                eventID,
                organiserID,
                eventType,
                eventName,
                eventDate,
                eventLocation,
                eventDescription);
        }

        // DELETE EVENT
        [HttpDelete("{eventID}")]
        public string DeleteEvent(int eventID)
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
                return "Only Organisers can delete events.";
            }

            int organiserID =
                GetOrganiserID(userID.Value);

            if (organiserID == 0)
            {
                return "Organiser account not found.";
            }

            event_info events = new event_info();

            return events.delete_event(
                eventID,
                organiserID);
        }


        // Converts UserID to OrganiserID
        private int GetOrganiserID(int userID)
        {
            int organiserID = 0;

            string connectionString = @"Data source=(localdb)\api_guide;database=Race_Day;";

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
