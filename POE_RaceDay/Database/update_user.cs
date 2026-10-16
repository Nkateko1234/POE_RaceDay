using System.Data.SqlClient;
namespace POE_RaceDay.Database
{
    public class update_user
    {
        private string connectionString = @"Data source=(localdb)\api_guide;database=Race_Day;";

        public string update_info(
            int userID,
            string firstName,
            string lastName,
            string email)
        {
            string message =
                "User information failed to update.";

            try
            {
                using (SqlConnection connection =
                    new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        UPDATE Users
                        SET FirstName = @FirstName,
                            LastName = @LastName,
                            Email = @Email
                        WHERE UserID = @UserID";

                    SqlCommand command =
                        new SqlCommand(query, connection);

                    command.Parameters.AddWithValue(
                        "@FirstName", firstName);

                    command.Parameters.AddWithValue(
                        "@LastName", lastName);

                    command.Parameters.AddWithValue(
                        "@Email", email);

                    command.Parameters.AddWithValue(
                        "@UserID", userID);

                    int rows =
                        command.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        message =
                            "User information updated successfully.";
                    }
                    else
                    {
                        message = "User not found.";
                    }
                }
            }
            catch (Exception ex)
            {
                message =
                    "Update failed: " + ex.Message;
            }

            return message;
        }
    }
}
