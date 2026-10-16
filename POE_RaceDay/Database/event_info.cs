using System.Data.SqlClient;

namespace POE_RaceDay.Database
{//start of namespace
    public class event_info
    {//start of event_info class

        //cnnectionstring
        private string connectionString = @"Data source=(localdb)\api_guide;database=Race_Day;";

        public string get_events()
        {//start of get_events
            string message = "";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            EventID,
                            OrganiserID,
                            EventType,
                            EventName,
                            EventDate,
                            EventLocation,
                            EventDescription
                        FROM Event";

                    SqlCommand command = new SqlCommand(query, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        message +=
                            "Event ID: " + reader["EventID"] +
                            " | Organiser ID: " + reader["OrganiserID"] +
                            " | Type: " + reader["EventType"] +
                            " | Name: " + reader["EventName"] +
                            " | Date: " + reader["EventDate"] +
                            " | Location: " + reader["EventLocation"] +
                            " | Description: " + reader["EventDescription"] +
                            Environment.NewLine;
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                message = "Failed to get events: " + ex.Message;
            }

            return message;
        }//end of get_events



        //get event
        public string get_event(int eventID)
        {//start of get_event
            string message = "Event not found.";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            EventID,
                            OrganiserID,
                            EventType,
                            EventName,
                            EventDate,
                            EventLocation,
                            EventDescription
                        FROM Event
                        WHERE EventID = @EventID";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue( "@EventID", eventID);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        message =
                            "Event ID: " + reader["EventID"] +
                            " | Organiser ID: " + reader["OrganiserID"] +
                            " | Type: " + reader["EventType"] +
                            " | Name: " + reader["EventName"] +
                            " | Date: " + reader["EventDate"] +
                            " | Location: " + reader["EventLocation"] +
                            " | Description: " + reader["EventDescription"];
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                message = "Failed to get event: " + ex.Message;
            }

            return message;
        }//start of get_event


        //create an event
        public string create_event( int organiserID,string eventType, string eventName, DateTime eventDate, string eventLocation, string eventDescription)
        {
            string message = "Event failed to create.";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO Event
                        (
                            OrganiserID,
                            EventType,
                            EventName,
                            EventDate,
                            EventLocation,
                            EventDescription
                        )
                        VALUES
                        (
                            @OrganiserID,
                            @EventType,
                            @EventName,
                            @EventDate,
                            @EventLocation,
                            @EventDescription
                        )";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue( "@OrganiserID", organiserID);

                    command.Parameters.AddWithValue( "@EventType", eventType);

                    command.Parameters.AddWithValue( "@EventName", eventName);

                    command.Parameters.AddWithValue( "@EventDate", eventDate);

                    command.Parameters.AddWithValue( "@EventLocation", eventLocation);

                    command.Parameters.AddWithValue("@EventDescription", eventDescription);

                    int rows =command.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        message = "Event created successfully.";
                    }
                }
            }
            catch (Exception ex)
            {
                message = "Event creation failed: " + ex.Message;
            }

            return message;
        }


        //update an event
        public string update_event( int eventID, int organiserID, string eventType, string eventName, DateTime eventDate, string eventLocation, string eventDescription)
        {//start of update_event
            string message = "Event failed to update.";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        UPDATE Event
                        SET
                            EventType = @EventType,
                            EventName = @EventName,
                            EventDate = @EventDate,
                            EventLocation = @EventLocation,
                            EventDescription = @EventDescription
                        WHERE
                            EventID = @EventID
                            AND OrganiserID = @OrganiserID";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue( "@EventID", eventID);

                    command.Parameters.AddWithValue( "@OrganiserID", organiserID);

                    command.Parameters.AddWithValue( "@EventType", eventType);

                    command.Parameters.AddWithValue( "@EventName", eventName);

                    command.Parameters.AddWithValue( "@EventDate", eventDate);

                    command.Parameters.AddWithValue( "@EventLocation", eventLocation);

                    command.Parameters.AddWithValue( "@EventDescription", eventDescription);

                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        message = "Event updated successfully.";
                    }
                    else
                    {
                        message = "Event not found or you do not own this event.";
                    }
                }
            }
            catch (Exception ex)
            {
                message = "Event update failed: " + ex.Message;
            }

            return message;
        }//end of update_event


      
        //delete an event
        public string delete_event( int eventID, int organiserID)
        {//start of delete_event
            string message = "Event failed to delete.";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        DELETE FROM Event
                        WHERE
                            EventID = @EventID
                            AND OrganiserID = @OrganiserID";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@EventID", eventID);

                    command.Parameters.AddWithValue("@OrganiserID", organiserID);

                    int rows =command.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        message = "Event deleted successfully.";
                    }
                    else
                    {
                        message =
                            "Event not found or you do not own this event.";
                    }
                }
            }
            catch (Exception ex)
            {
                message = "Event deletion failed: " + ex.Message;
            }

            return message;
        }//end of delete_event
    }//end of event_info class
}//end of namespace
