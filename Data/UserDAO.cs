using Microsoft.AspNetCore.Identity;
using MySqlConnector;
using UJConnect.Models;

//for SMTP
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

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
            const string sql = "SELECT UserID, Username, StudentEmail, HashPassword, CreatedAt, NumReports FROM AppUser WHERE StudentEmail = @StudentEmail";

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
                " VALUES (@username, @studentEmail, @hashPassword)";
            using var command = new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@studentEmail", user.StudentEmail);
            command.Parameters.AddWithValue("@hashPassword", user.PasswordHash);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public Boolean EmailAlreadyExists(String studentEmail)
        {
            //sql query to find number of users registered with this email
            string sql = "SELECT COUNT(*) FROM AppUser WHERE StudentEmail = @Email";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", studentEmail);
            long count = (long)command.ExecuteScalar();

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
            long count = (long)command.ExecuteScalar();

            return count > 0;
        }

        public bool sendVerificationLink(User user, string smtpHost, int smtpPort, string smtpUsername, string smtpPassword) 
        {
            string token = Guid.NewGuid().ToString("N"); // random, unguessable

            //sql query to set the verification toek
            const string sql = "UPDATE AppUser SET VerificationToken = @Token WHERE UserID = @UserID";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            //execute the query
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Token", token);
            command.Parameters.AddWithValue("@UserID", user.UserID);
            command.ExecuteNonQuery();

            string verificationLink = $"https://campusMate.com/Verify/Confirm?token={token}";

            //construct the email
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("campusMate", "noreply@campusMate.com"));
            message.To.Add(new MailboxAddress(user.Username, user.StudentEmail));
            message.Subject = "Verify your campusMate account";
            message.Body = new TextPart("plain")
            {
                Text = $"Hi {user.Username}, \n\nVerify your account:\n{verificationLink}"
            };

            using var client = new SmtpClient();
            client.Connect(smtpHost, smtpPort, SecureSocketOptions.StartTls);
            client.Authenticate(smtpUsername, smtpPassword);
            client.Send(message);
            client.Disconnect(true);

            return true; ;
        }

        public bool ConfirmVerificationToken(string token)
        {
            //sql query to find user with matching token
            string sql = "SELECT * FROM AppUser WHERE VerificationToken = @Token";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            //execute the query
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Token", token);

            int userID;
            using (var reader = command.ExecuteReader()) //reader is the user with the specified token
            {
                if (!reader.Read())
                {
                    return false; //token basically inalid or DNE
                }
                userID = reader.GetInt32("UserID");
            }
            

            //sql query to (1) update UerVerified and (2) delete the token
            string updateSql = "UPDATE AppUser SET UserVerified = true, VerificationToken = NULL WHERE UserID = @UserID";
            using var updateCommand = new MySqlCommand(updateSql, connection);
            updateCommand.Parameters.AddWithValue("@UserID", userID);
            updateCommand.ExecuteNonQuery();

            return true;
        }

        public bool sendResetPasswordLink(User user)
        {
            return true;
        }

        public User? FindUserByResetToken(string token)
        {
            return null;
        }

        public bool ResetPassword(User user, String newPassword)
        {
            //hash the password first
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

            if (BCrypt.Net.BCrypt.Verify(newPassword, user.PasswordHash))
            {
                return false; //new password cannot be old password
            }

            //sql query to change password
            string sql = "UPDATE AppUser SET HashPassword = @Password WHERE StudentEmail = @StudentEmail";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            //execute
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Password", hashedPassword);
            command.Parameters.AddWithValue("@StudentEmail", user.StudentEmail);
            command.ExecuteNonQuery();

            return true;
        }

        public bool ChangeUsername(User user, String newUsername)
        {
            if (newUsername.Equals(user.Username))
            {
                return false; //no change
            }

            //sql query to change password
            string sql = "UPDATE AppUser SET Username = @Username WHERE StudentEmail = @StudentEmail";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            //execute
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Username", newUsername);
            command.Parameters.AddWithValue("@StudentEmail", user.StudentEmail);
            command.ExecuteNonQuery();

            //update the object
            user.Username = newUsername;

            return true;
        }

        public bool deleteAccount(User user, string plainPassword) {
            bool passwordverified = BCrypt.Net.BCrypt.Verify(plainPassword, user.PasswordHash);
            if (!passwordverified)
                return false; //stop reset if user cant verify their password
            
            //sql query to delete AppUser
            string sql = "DELETE FROM AppUser WHERE StudentEmail = @StudentEmail";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            //execute
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@StudentEmail", user.StudentEmail);

            return true;
        }

        public User? SearchUser(String usernameOrStudentEmail)
        {
            return null;
        }

        public User GetUserByID(int userID) {
            User user = null;
            
            return user;
        }

        public void IncrementReportCount(int userID)
        {

        }
    }
}
