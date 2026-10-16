using System.Data.SqlClient;
namespace POE_RaceDay.Database
{//start of namespace
    public class get_user
    {//start of get_user class
        private string connectionString = @"Data source=(localdb)\api_guide;database=Race_Day;";

        public string get_info(int userID)
        {//tstart of get_info method
            string message = "User not found.";

            try
            {//start of try
                using (SqlConnection connection = new SqlConnection(connectionString))
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

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue( "@UserID", userID);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        //displays user's info
                        message = "User ID: " + reader["UserID"] + " | First Name: " + reader["FirstName"] + " | Last Name: " + reader["LastName"] +  " | Email: " + reader["Email"] + " | Role: " + reader["Role"];
                    }

                    reader.Close();
                }
            }//end of try
            catch (Exception ex)
            {//start of catch
                //return message
                message = "Failed to get user: " + ex.Message;
            }//end of catch

            return message;
        }//end of method
    }//end of get_user class
}//end of namespace
