using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
namespace POE_RaceDay.Database
{
    public class login_user
    {
        private string connectionString = @"Data source=(localdb)\api_guide;database=Race_Day;";

        public string login(
           string email,
           string password,
           out int userID,
           out string role)
        {
            userID = 0;
            role = "";

            string message = "Incorrect email or password.";

            try
            {
                using (SqlConnection connection =
                    new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT UserID, Password, Role
                        FROM Users
                        WHERE Email = @Email";

                    SqlCommand command =
                        new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Email", email);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string storedPassword =
                            reader["Password"].ToString();

                        role = reader["Role"].ToString();
                        userID = Convert.ToInt32(reader["UserID"]);

                        // Hash the entered password
                        using (SHA256 sha256 = SHA256.Create())
                        {
                            byte[] bytes =
                                Encoding.UTF8.GetBytes(password);

                            byte[] hash =
                                sha256.ComputeHash(bytes);

                            string hashedPassword =
                                Convert.ToBase64String(hash);

                            if (hashedPassword == storedPassword)
                            {
                                message = "Login successful.";
                            }
                            else
                            {
                                userID = 0;
                                role = "";
                            }
                        }
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                message = "Login failed: " + ex.Message;
            }

            return message;
        }
    }
}
