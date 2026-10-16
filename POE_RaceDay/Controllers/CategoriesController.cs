using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POE_RaceDay.Database;
using System.Data.SqlClient;

namespace POE_RaceDay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private string connectionString = @"Data source=(localdb)\api_guide;database=Race_Day;";

        [HttpGet]
        public string GetCategories()
        {
            category_info category =
                new category_info();

            return category.get_categories();
        }


        // =====================================================
        // GET CATEGORIES FOR AN EVENT
        // =====================================================

        [HttpGet("event/{eventID}")]
        public string GetEventCategories(int eventID)
        {
            category_info category =
                new category_info();

            return category.get_event_categories(
                eventID);
        }


        // =====================================================
        // CREATE CATEGORY
        // ORGANISER ONLY
        // =====================================================

        [HttpPost]
        public string CreateCategory(
            int eventID,
            string categoryName,
            string distance,
            decimal entryFee)
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
                    "Only Organisers can create categories.";
            }

            int organiserID =
                GetOrganiserID(userID.Value);

            if (organiserID == 0)
            {
                return "Organiser account not found.";
            }

            // Check that the event belongs to this organiser
            if (!EventBelongsToOrganiser(
                    eventID, organiserID))
            {
                return
                    "You can only create categories for your own events.";
            }

            category_info category =
                new category_info();

            return category.create_category(
                eventID,
                categoryName,
                distance,
                entryFee);
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


        // =====================================================
        // CHECK EVENT OWNERSHIP
        // =====================================================

        private bool EventBelongsToOrganiser(
            int eventID,
            int organiserID)
        {
            try
            {
                using (SqlConnection connection =
                    new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT COUNT(*)
                        FROM Event
                        WHERE EventID = @EventID
                        AND OrganiserID = @OrganiserID";

                    SqlCommand command =
                        new SqlCommand(query, connection);

                    command.Parameters.AddWithValue(
                        "@EventID", eventID);

                    command.Parameters.AddWithValue(
                        "@OrganiserID", organiserID);

                    int count =
                        Convert.ToInt32(
                            command.ExecuteScalar());

                    return count > 0;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}
