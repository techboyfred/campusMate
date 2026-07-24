using Microsoft.AspNetCore.Identity;
using MySqlConnector;
using UJConnect.Models;

namespace UJConnect.Data
{
    public class UserDAO
    {
        private readonly String _connectionString;

        public UserDAO(String connectionString)
        {
            _connectionString = connectionString;
        }

        //login method
        public User? LoginWithStudentEmail(String studentEmail, String plainPassword)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            //sql query to get ALL the user's info from thr database
            const string sql = "SELECT UserID, Username, StudentEmail, HashPassword, CreatedAt, NumReports FROM AppUser WHERE StudentEmail = @Email";

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@StudentEmail", studentEmail); //execute query with given email
            using var reader = command.ExecuteReader();

            if (!reader.Read())
            {
                //user with that email not found
                return null;
            }

            String storedHash = reader.GetString("HashPassword");
            bool passwordMatches = BCrypt.Net.BCrypt.Verify(plainPassword, storedHash);

            if (!passwordMatches)
            {
                return null;
            }

            return new User(
                userID: reader.GetInt32("UserID"),
                username: reader.GetString("Username"),
                studentEmail: reader.GetString("StudentEmail"),
                passwordHash: storedHash,
                createdAt: reader.GetDateTime("CreatedAt"),
                numReports: reader.GetInt32("NumReports")

             );
        }

        public User? LoginWithUsername(String username, String plainPassword)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            //sql query to get ALL the user's info from thr database
            const string sql = "SELECT UserID, Username, StudentEmail, HashPassword, CreatedAt, NumReports FROM AppUser WHERE Username = @Username";

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Username", username); //execute query with given email
            using var reader = command.ExecuteReader();

            if (!reader.Read())
            {
                //user with that email not found
                return null;
            }

            String storedHash = reader.GetString("HashPassword");
            bool passwordMatches = BCrypt.Net.BCrypt.Verify(plainPassword, storedHash);

            if (!passwordMatches)
            {
                return null;
            }

            return new User(
                userID: reader.GetInt32("UserID"),
                username: reader.GetString("Username"),
                studentEmail: reader.GetString("StudentEmail"),
                passwordHash: storedHash,
                createdAt: reader.GetDateTime("CreatedAt"),
                numReports: reader.GetInt32("NumReports")

             );
        }

        public bool Register(User user)
        {
            if (EmailAlreadyExists(user.StudentEmail))
                return false;

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            //sql query to register an account into the database
            const string sql = "INSERT INTO AppUser (Username, StudentEmail, HashPassword)" +
                "VALUES (@username, @studentEmail, @hashPassword)";
            using var command = new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@studentEmail", user.StudentEmail);
            command.Parameters.AddWithValue("@hashPassword", user.PasswordHash);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public User? SearchUser(String usernameOrStudentEmail)
        {
            return null;
        }

        public Boolean EmailAlreadyExists(String studentEmail)
        {
            //sql query to find number of users registered with this email
            string sql = "SELECT COUNT(*) FROM AppUser WHERE StudentEmail = @Email";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", studentEmail);
            int count = (int)command.ExecuteScalar();

            return count > 0;
        }

        public Boolean UsernameAlreadyExists(String username)
        {
            //sql query to find number of users registered with this username
            string sql = "SELECT COUNT(*) FROM AppUser WHERE Username = @Username";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Username", username);
            int count = (int)command.ExecuteScalar();

            return count > 0;
        }

        public bool sendVerificationLink(User user) 
        {
            return true;
        }

        public bool sendResetPasswordLink(User user)
        {
            return true;
        }

        public bool ResetPassword(User user, String newPassword)
        {
            //hash the password first
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

            //sql query to change password
            string sql = "UPDATE AppUser SET PasswordHash = @Password WHERE StudentEmail = @StudentEmail";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            //execute
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Password", hashedPassword);
            command.Parameters.AddWithValue("@StudentEmail", user.StudentEmail);

            return true;
        }

        public bool deleteAccount(User user) {
            //sql query to delete AppUser
            string sql = "DELETE FROM AppUser WHERE StudentEmail = @StudentEmail";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            //execute
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@StudentEmail", user.StudentEmail);

            return true;
        }
    }
}
