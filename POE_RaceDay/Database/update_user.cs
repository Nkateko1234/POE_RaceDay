using System.Data.SqlClient;
namespace POE_RaceDay.Database
{//start of namespace
    public class update_user
    {//start of update_user

        //connection string
        private string connectionString = @"Data source=(localdb)\api_guide;database=Race_Day;";

        public string update_info(int userID, string firstName, string lastName, string email)
        {//start of update_info
            string message = "User information failed to update.";

            try
            {//start of try

                //using SQLConnection
                using (SqlConnection connection =new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @" UPDATE Users
                        SET FirstName = @FirstName,
                            LastName = @LastName,
                            Email = @Email
                        WHERE UserID = @UserID";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@FirstName", firstName);

                    command.Parameters.AddWithValue( "@LastName", lastName);

                    command.Parameters.AddWithValue( "@Email", email);

                    command.Parameters.AddWithValue( "@UserID", userID);

                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        //message if update is successfull
                        message = "User information updated successfully.";
                    }
                    else
                    {
                        //return message if user is not found
                        message = "User not found.";
                    }
                }
            }//end of try 
            catch (Exception ex)
            {//start of catch
                message = "Update failed: " + ex.Message;
            }//end of catch

            return message;
        }//end of update_info
    }//end of class
}//end of namespace
