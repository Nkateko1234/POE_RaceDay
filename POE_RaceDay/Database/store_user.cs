using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
namespace POE_RaceDay.Database
{//start of namespace
    public class store_user
    {//start of store_user

        //connection string
        private string connectionString = @"Data source=(localdb)\api_guide;database=Race_Day;";

        public string register_user( string firstName, string lastName, string email, string password, string role)
        {//start of register_uer method
            string message = "Registration failed.";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Hash the password
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(password);
                        byte[] hash = sha256.ComputeHash(bytes);

                        password = Convert.ToBase64String(hash);
                    }

                    string query = @"
                        INSERT INTO Users
                        (FirstName, LastName, Email, Password, Role)
                        VALUES
                        (@FirstName, @LastName, @Email, @Password, @Role);

                        SELECT SCOPE_IDENTITY();";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Role", role);

                    int userID = Convert.ToInt32(command.ExecuteScalar());

                    // Create the matching Organiser or Participant record
                    if (role == "Organiser")
                    {
                        string organiserQuery = @"
                            INSERT INTO Organiser
                            (UserID, OrganiserName, OrganiserSurname, Email)
                            VALUES
                            (@UserID, @Name, @Surname, @Email)";

                        SqlCommand organiserCommand = new SqlCommand(organiserQuery, connection);

                        organiserCommand.Parameters.AddWithValue("@UserID", userID);
                        organiserCommand.Parameters.AddWithValue("@Name", firstName);
                        organiserCommand.Parameters.AddWithValue("@Surname", lastName);
                        organiserCommand.Parameters.AddWithValue("@Email", email);

                        organiserCommand.ExecuteNonQuery();
                    }
                    else if (role == "Participant")
                    {
                        string participantQuery = @"
                            INSERT INTO Participants
                            (UserID, PName, Surname, Email)
                            VALUES
                            (@UserID, @Name, @Surname, @Email)";

                        SqlCommand participantCommand = new SqlCommand(participantQuery, connection);

                        participantCommand.Parameters.AddWithValue("@UserID", userID);
                        participantCommand.Parameters.AddWithValue("@Name", firstName);
                        participantCommand.Parameters.AddWithValue("@Surname", lastName);
                        participantCommand.Parameters.AddWithValue("@Email", email);

                        participantCommand.ExecuteNonQuery();
                    }

                    message = "Registration successful. User ID: " + userID;
                }
            }
            catch (Exception ex)
            {
                message = "Registration failed: " + ex.Message;
            }

            return message;
        }//end of register_uer method
    }//end of store_user
}//end of namespace
