using System;
using System.Data;
using Microsoft.Data.SqlClient;


namespace SQLconsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=ifonvizin_;User Id=ifonvizin_;Password=12345;TrustServerCertificate=True;";

            bool isAuthenticated = false;


            while (!isAuthenticated)
            {
                Console.WriteLine("Choose an operation:");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RegisterUser(connectionString);
                        break;
                    case "2":
                        isAuthenticated = LoginUser(connectionString);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }

            while (isAuthenticated)
            {
                Console.WriteLine("Choose an operation:");
                Console.WriteLine("1. Add User");
                Console.WriteLine("2. Get Users");
                Console.WriteLine("3. Update User");
                Console.WriteLine("4. Delete User");
                Console.WriteLine("5. Search Articles by Category");
                Console.WriteLine("6. Search Articles by Tag");
                Console.WriteLine("7. Logout");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddUser(connectionString);
                        break;
                    case "2":
                        GetUsers(connectionString);
                        break;
                    case "3":
                        UpdateUser(connectionString);
                        break;
                    case "4":
                        DeleteUser(connectionString);
                        break;
                    case "5":
                        SearchArticlesByCategory(connectionString);
                        break;
                    case "6":
                        SearchArticlesByTag(connectionString);
                        break;
                    case "7":
                        isAuthenticated = false;
                        Console.WriteLine("Logged out successfully.");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void RegisterUser(string connectionString)
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter email: ");
            string email = Console.ReadLine();
            Console.Write("Enter password: ");
            string passwordHash = Console.ReadLine();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Users (username, email, password_hash) VALUES (@Username, @Email, @PasswordHash)", connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                command.ExecuteNonQuery();
                Console.WriteLine("User registered successfully.");
            }
        }

        static bool LoginUser(string connectionString)
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter password hash: ");
            string passwordHash = Console.ReadLine();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Users WHERE username = @Username AND password_hash = @PasswordHash AND is_deleted = 0", connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.WriteLine("Login successful.");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid username or password.");
                        return false;
                    }
                }
            }
        }

        static void AddUser(string connectionString)
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter email: ");
            string email = Console.ReadLine();
            Console.Write("Enter password: ");
            string passwordHash = Console.ReadLine();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Users (username, email, password_hash) VALUES (@Username, @Email, @PasswordHash)", connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                command.ExecuteNonQuery();
                Console.WriteLine("User added successfully.");
            }
        }

        static void GetUsers(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Users WHERE is_deleted = 0", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"User ID: {reader.GetInt32(reader.GetOrdinal("user_id"))}, Username: {reader.GetString(reader.GetOrdinal("username"))}, Email: {reader.GetString(reader.GetOrdinal("email"))}");
                    }
                }
            }
        }

        static void UpdateUser(string connectionString)
        {
            Console.Write("Enter user ID to update: ");
            int userId = int.Parse(Console.ReadLine());
            Console.Write("Enter new username: ");
            string username = Console.ReadLine();
            Console.Write("Enter new email: ");
            string email = Console.ReadLine();
            Console.Write("Enter new password: ");
            string passwordHash = Console.ReadLine();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE Users SET username = @Username, email = @Email, password_hash = @PasswordHash WHERE user_id = @UserId AND is_deleted = 0", connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                command.Parameters.AddWithValue("@UserId", userId);
                command.ExecuteNonQuery();
                Console.WriteLine("User updated successfully.");
            }
        }

        static void DeleteUser(string connectionString)
        {
            Console.Write("Enter user ID to delete: ");
            int userId = int.Parse(Console.ReadLine());

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE Users SET is_deleted = 1 WHERE user_id = @UserId AND is_deleted = 0", connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.ExecuteNonQuery();
                Console.WriteLine("User marked as deleted successfully.");
            }
        }

        static void SearchArticlesByCategory(string connectionString)
        {
            Console.Write("Enter category ID to search: ");
            int categoryId = int.Parse(Console.ReadLine());

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Articles WHERE category_id = @CategoryId AND is_published = 1", connection);
                command.Parameters.AddWithValue("@CategoryId", categoryId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Article ID: {reader.GetInt32(reader.GetOrdinal("article_id"))}, Title: {reader.GetString(reader.GetOrdinal("title"))}, Content: {reader.GetString(reader.GetOrdinal("content"))}");
                    }
                }
            }
        }

        static void SearchArticlesByTag(string connectionString)
        {
            Console.Write("Enter tag ID to search: ");
            int tagId = int.Parse(Console.ReadLine());

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT a.* FROM Articles a JOIN Article_Tags at ON a.article_id = at.article_id WHERE at.tag_id = @TagId AND is_published = 1", connection);
                command.Parameters.AddWithValue("@TagId", tagId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Article ID: {reader.GetInt32(reader.GetOrdinal("article_id"))}, Title: {reader.GetString(reader.GetOrdinal("title"))}, Content: {reader.GetString(reader.GetOrdinal("content"))}");
                    }
                }
            }
        }
    }
}