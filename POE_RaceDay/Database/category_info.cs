using System.Data.SqlClient;

namespace POE_RaceDay.Database
{
    public class category_info
    {
        private string connectionString = @"Data source=(localdb)\api_guide;database=Race_Day;";

        public string get_categories()
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
                            CategoryID,
                            EventID,
                            CategoryName,
                            Distance,
                            EntryFee
                        FROM Category";

                    SqlCommand command =
                        new SqlCommand(query, connection);

                    SqlDataReader reader =
                        command.ExecuteReader();

                    while (reader.Read())
                    {
                        message +=
                            "Category ID: " +
                            reader["CategoryID"] +
                            " | Event ID: " +
                            reader["EventID"] +
                            " | Name: " +
                            reader["CategoryName"] +
                            " | Distance: " +
                            reader["Distance"] +
                            " | Entry Fee: " +
                            reader["EntryFee"] +
                            Environment.NewLine;
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                message =
                    "Failed to get categories: " +
                    ex.Message;
            }

            return message;
        }


        // =====================================================
        // GET CATEGORIES FOR AN EVENT
        // =====================================================

        public string get_event_categories(int eventID)
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
                            CategoryID,
                            EventID,
                            CategoryName,
                            Distance,
                            EntryFee
                        FROM Category
                        WHERE EventID = @EventID";

                    SqlCommand command =
                        new SqlCommand(query, connection);

                    command.Parameters.AddWithValue(
                        "@EventID", eventID);

                    SqlDataReader reader =
                        command.ExecuteReader();

                    while (reader.Read())
                    {
                        message +=
                            "Category ID: " +
                            reader["CategoryID"] +
                            " | Name: " +
                            reader["CategoryName"] +
                            " | Distance: " +
                            reader["Distance"] +
                            " | Entry Fee: " +
                            reader["EntryFee"] +
                            Environment.NewLine;
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                message =
                    "Failed to get categories: " +
                    ex.Message;
            }

            return message;
        }


        // =====================================================
        // CREATE CATEGORY
        // =====================================================

        public string create_category(
            int eventID,
            string categoryName,
            string distance,
            decimal entryFee)
        {
            string message =
                "Category failed to create.";

            try
            {
                using (SqlConnection connection =
                    new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO Category
                        (
                            EventID,
                            CategoryName,
                            Distance,
                            EntryFee
                        )
                        VALUES
                        (
                            @EventID,
                            @CategoryName,
                            @Distance,
                            @EntryFee
                        )";

                    SqlCommand command =
                        new SqlCommand(query, connection);

                    command.Parameters.AddWithValue(
                        "@EventID", eventID);

                    command.Parameters.AddWithValue(
                        "@CategoryName", categoryName);

                    command.Parameters.AddWithValue(
                        "@Distance", distance);

                    command.Parameters.AddWithValue(
                        "@EntryFee", entryFee);

                    int rows =
                        command.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        message =
                            "Category created successfully.";
                    }
                }
            }
            catch (Exception ex)
            {
                message =
                    "Category creation failed: " +
                    ex.Message;
            }

            return message;
        }
    }
}
