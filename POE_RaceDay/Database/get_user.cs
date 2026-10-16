using System.Data.SqlClient;
namespace POE_RaceDay.Database
{
    public class get_user
    {
        private string connectionString = @"Data source=(localdb)\api_guide;database=Race_Day;";

        public string get_info(int userID)
        {
            string message = "User not found.";

            try
            {
                using (SqlConnection connection =
                    new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT UserID,
                               FirstName,
                               LastName,
                               Email,
                               Role
                        FROM Users
                        WHERE UserID = @UserID";

                    SqlCommand command =
                        new SqlCommand(query, connection);

                    command.Parameters.AddWithValue(
                        "@UserID", userID);

                    SqlDataReader reader =
                        command.ExecuteReader();

                    if (reader.Read())
                    {
                        message =
                            "User ID: " +
                            reader["UserID"] +
                            " | First Name: " +
                            reader["FirstName"] +
                            " | Last Name: " +
                            reader["LastName"] +
                            " | Email: " +
                            reader["Email"] +
                            " | Role: " +
                            reader["Role"];
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                message =
                    "Failed to get user: " +
                    ex.Message;
            }

            return message;
        }
    }
}
