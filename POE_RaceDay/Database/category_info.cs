using System.Data.SqlClient;
using POE_RaceDay.Models;

namespace POE_RaceDay.Database
{
    public class category_info
    {
        private string connectionString = @"Data source=(localdb)\api_guide;database=Race_Day;";

        public List<CategoryModel> get_categories()
        {
            List<CategoryModel> categories = new List<CategoryModel>();

                using (SqlConnection connection =new SqlConnection(connectionString))
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

                    SqlCommand command = new SqlCommand(query, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                    CategoryModel category = new CategoryModel();

                    category.CategoryID = Convert.ToInt32(reader["CategoryID"]);
                    category.EventID = Convert.ToInt32(reader["EventID"]);
                    category.CategoryName = reader["CategoryName"].ToString() ?? "";
                    category.Distance = reader["Distance"].ToString() ?? "";
                    category.EntryFee = Convert.ToDecimal(reader["EntryFee"]);

                    categories.Add(category);
                }

                    reader.Close();
                }
            return categories;
            }
            

        // =====================================================
        // GET CATEGORIES FOR AN EVENT
        // =====================================================

        public List<CategoryModel> get_event_categories(int eventID)
        {
            List<CategoryModel> categories = new List<CategoryModel>();

                using (SqlConnection connection =new SqlConnection(connectionString))
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

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue( "@EventID", eventID);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                    CategoryModel category = new CategoryModel();

                    category.CategoryID = Convert.ToInt32(reader["CategoryID"]);
                    category.EventID = Convert.ToInt32(reader["EventID"]);
                    category.CategoryName = reader["CategoryName"].ToString() ?? "";
                    category.Distance = reader["Distance"].ToString() ?? "";
                    category.EntryFee = Convert.ToDecimal(reader["EntryFee"]);

                    categories.Add(category);
                }

                    reader.Close();
                }
            return categories;
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
