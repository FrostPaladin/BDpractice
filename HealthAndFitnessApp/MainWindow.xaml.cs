using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;

namespace HealthAndFitnessApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string _connectionString;

        public MainWindow()
        {
            InitializeComponent();
            _connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=ifonvizin_;User Id=ifonvizin_;Password=12345;TrustServerCertificate=True;";
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            spRegister.Visibility = Visibility.Visible;
            spLogin.Visibility = Visibility.Collapsed;
            spMain.Visibility = Visibility.Collapsed;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            spLogin.Visibility = Visibility.Visible;
            spRegister.Visibility = Visibility.Collapsed;
            spMain.Visibility = Visibility.Collapsed;
        }

        private void btnSubmitRegister_Click(object sender, RoutedEventArgs e)
        {
            string username = txtRegisterUsername.Text;
            string email = txtRegisterEmail.Text;
            string passwordHash = txtRegisterPasswordHash.Text;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Users (username, email, password_hash) VALUES (@Username, @Email, @PasswordHash)", connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                command.ExecuteNonQuery();
                MessageBox.Show("Пользователь зарегистрирован.");
                spRegister.Visibility = Visibility.Collapsed;
                spMain.Visibility = Visibility.Visible;

            }
        }

        private void btnSubmitLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtLoginUsername.Text;
            string passwordHash = txtLoginPasswordHash.Password;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Users WHERE username = @Username AND password_hash = @PasswordHash AND is_deleted = 0", connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        MessageBox.Show("Логин успешен.");
                        spLogin.Visibility = Visibility.Collapsed;
                        spMain.Visibility = Visibility.Visible;
                        spBase.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль.");
                    }
                }
            }
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            spAddUser.Visibility = Visibility.Visible;
            spMain.Visibility = Visibility.Collapsed;
        }

        private void btnSubmitAddUser_Click(object sender, RoutedEventArgs e)
        {
            string username = txtAddUsername.Text;
            string email = txtAddEmail.Text;
            string passwordHash = txtAddPasswordHash.Text;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Users (username, email, password_hash) VALUES (@Username, @Email, @PasswordHash)", connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                command.ExecuteNonQuery();
                MessageBox.Show("Пользователь добавлен успешно.");
                spAddUser.Visibility = Visibility.Collapsed;
                spMain.Visibility = Visibility.Visible;
            }
        }


        private void btnGetUsers_Click(object sender, RoutedEventArgs e)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Users WHERE is_deleted = 0", connection);
                using (var reader = command.ExecuteReader())
                {
                    lbUsers.Items.Clear();
                    while (reader.Read())
                    {
                        lbUsers.Items.Add($"User ID: {reader.GetInt32(reader.GetOrdinal("user_id"))}, Username: {reader.GetString(reader.GetOrdinal("username"))}, Email: {reader.GetString(reader.GetOrdinal("email"))}");
                    }
                }
            }
            spGetUsers.Visibility = Visibility.Visible;
            spMain.Visibility = Visibility.Collapsed;
        }

        private void btnBackFromGetUsers_Click(object sender, RoutedEventArgs e)
        {
            spGetUsers.Visibility = Visibility.Collapsed;
            spMain.Visibility = Visibility.Visible;
        }

        private void btnUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            spUpdateUser.Visibility = Visibility.Visible;
            spMain.Visibility = Visibility.Collapsed;
        }

        private void btnSubmitUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            int userId = int.Parse(txtUpdateUserId.Text);
            string username = txtUpdateUsername.Text;
            string email = txtUpdateEmail.Text;
            string passwordHash = txtUpdatePasswordHash.Text;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE Users SET username = @Username, email = @Email, password_hash = @PasswordHash WHERE user_id = @UserId AND is_deleted = 0", connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                command.Parameters.AddWithValue("@UserId", userId);
                command.ExecuteNonQuery();
                MessageBox.Show("Информация обновлена успешно.");
                spUpdateUser.Visibility = Visibility.Collapsed;
                spMain.Visibility = Visibility.Visible;
            }
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            spDeleteUser.Visibility = Visibility.Visible;
            spMain.Visibility = Visibility.Collapsed;
        }

        private void btnSubmitDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            int userId = int.Parse(txtDeleteUserId.Text);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE Users SET is_deleted = 1 WHERE user_id = @UserId AND is_deleted = 0", connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.ExecuteNonQuery();
                MessageBox.Show("Пользователь удален успешно.");
                spDeleteUser.Visibility = Visibility.Collapsed;
                spMain.Visibility = Visibility.Visible;
            }
        }

        private void btnSearchArticlesByCategory_Click(object sender, RoutedEventArgs e)
        {
            spSearchArticlesByCategory.Visibility = Visibility.Visible;
            spMain.Visibility = Visibility.Collapsed;
        }

        private void btnSubmitSearchArticlesByCategory_Click(object sender, RoutedEventArgs e)
        {
            int categoryId = int.Parse(txtSearchCategoryId.Text);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Articles WHERE category_id = @CategoryId AND is_published = 1", connection);
                command.Parameters.AddWithValue("@CategoryId", categoryId);

                using (var reader = command.ExecuteReader())
                {
                    lbSearchArticlesByCategory.Items.Clear();
                    while (reader.Read())
                    {
                        lbSearchArticlesByCategory.Items.Add($"Article ID: {reader.GetInt32(reader.GetOrdinal("article_id"))}, Title: {reader.GetString(reader.GetOrdinal("title"))}, Content: {reader.GetString(reader.GetOrdinal("content"))}");
                    }
                }
            }
        }

        private void btnSearchArticlesByTag_Click(object sender, RoutedEventArgs e)
        {
            spSearchArticlesByTag.Visibility = Visibility.Visible;
            spMain.Visibility = Visibility.Collapsed;
        }

        private void btnSubmitSearchArticlesByTag_Click(object sender, RoutedEventArgs e)
        {
            int tagId = int.Parse(txtSearchTagId.Text);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT a.* FROM Articles a JOIN Article_Tags at ON a.article_id = at.article_id WHERE at.tag_id = @TagId AND a.is_published = 1", connection);
                command.Parameters.AddWithValue("@TagId", tagId);

                using (var reader = command.ExecuteReader())
                {
                    lbSearchArticlesByTag.Items.Clear();
                    while (reader.Read())
                    {
                        lbSearchArticlesByTag.Items.Add($"Article ID: {reader.GetInt32(reader.GetOrdinal("article_id"))}, Title: {reader.GetString(reader.GetOrdinal("title"))}, Content: {reader.GetString(reader.GetOrdinal("content"))}");
                    }
                }
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            spMain.Visibility = Visibility.Collapsed;
            spRegister.Visibility = Visibility.Collapsed;
            spLogin.Visibility = Visibility.Visible;
        }

        private void btnBackFromAddUser_Click(object sender, RoutedEventArgs e)
        {
            spAddUser.Visibility = Visibility.Collapsed;
            spMain.Visibility = Visibility.Visible;
        }

        private void btnBackFromUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            spUpdateUser.Visibility = Visibility.Collapsed;
            spMain.Visibility = Visibility.Visible;
        }

        private void btnBackFromDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            spDeleteUser.Visibility = Visibility.Collapsed;
            spMain.Visibility = Visibility.Visible;
        }

        private void btnBackFromSearchArticlesByCategory_Click(object sender, RoutedEventArgs e)
        {
            spSearchArticlesByCategory.Visibility = Visibility.Collapsed;
            spMain.Visibility = Visibility.Visible;
        }

        private void btnBackFromSearchArticlesByTag_Click(object sender, RoutedEventArgs e)
        {
            spSearchArticlesByTag.Visibility = Visibility.Collapsed;
            spMain.Visibility = Visibility.Visible;
        }
    }
}
