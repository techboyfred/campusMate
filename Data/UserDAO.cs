//for SMTP
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using MimeKit;
using MySqlConnector;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

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

        public bool isStudentEmail(string input) 
        {
            string pattern = "\\d{9}@student.uj.ac.za";
            Match match = Regex.Match(input, pattern);
            return match.Success;
        }

        public bool AccountExists(string usernameOrStudentEmail)
        {
            bool isEmail = isStudentEmail(usernameOrStudentEmail);

            //sql query to find number of users with username or email
            string sql;
            if (isEmail)
            { //using student email
                sql = "SELECT COUNT(*) FROM AppUser WHERE StudentEmail = @Value";
            }
            else
            { //using username
                sql = "SELECT COUNT(*) FROM AppUser WHERE Username = @Value";
            }

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Value", usernameOrStudentEmail);
            long count = (long)command.ExecuteScalar();

            return count > 0;
        }

        //login method
        public User? Login(string usernameOrStudentEmail, string plainPassword)
        {
            bool isEmail = isStudentEmail(usernameOrStudentEmail);
            string sql;
            if (isEmail) { //using student email
                sql = "SELECT UserID, Username, StudentEmail, HashPassword, CreatedAt, NumReports FROM AppUser WHERE StudentEmail = @Value";
            }
            else{ //using username
                sql = "SELECT UserID, Username, StudentEmail, HashPassword, CreatedAt, NumReports FROM AppUser WHERE Username = @Value";
            }

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Value", usernameOrStudentEmail);
            using var reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return null; // no matching user, by email or username
            }

            string storedHash = reader.GetString("HashPassword");
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

        public bool sendResetPasswordLink(User user, string smtpHost, int smtpPort, string smtpUsername, string smtpPassword)
        {
            string token = Guid.NewGuid().ToString("N");
            DateTime expiry = DateTime.UtcNow.AddMinutes(30); // link expires in 30min

            const string sql = "UPDATE AppUser SET ResetToken = @Token, ResetTokenExpiry = @Expiry WHERE UserID = @UserID";
            
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Token", token);
            command.Parameters.AddWithValue("@Expiry", expiry);
            command.Parameters.AddWithValue("@UserID", user.UserID);
            command.ExecuteNonQuery();

            string resetLink = $"https://campusmate.com/MarketLogin/ResetPassword?token={token}";

            //construct the email
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("campusMate", "noreply@campusmate.com"));
            message.To.Add(new MailboxAddress(user.Username, user.StudentEmail));
            message.Subject = "Reset your campusMate password";
            message.Body = new TextPart("plain")
            {
                Text = $"Hi {user.Username}, \n\nReset your password:\n{resetLink}\n\nThis link will expire in 30 minutes.\n\n" +
                "If you did not request a password reset, please ignore this email."
            };

            using var client = new SmtpClient();
            client.Connect(smtpHost, smtpPort, SecureSocketOptions.StartTls);
            client.Authenticate(smtpUsername, smtpPassword);
            client.Send(message);
            client.Disconnect(true);

            return true;
        }

        public User? FindUserByResetToken(string token)
        {
            //sql query to find user with matching reset token
            string sql = "SELECT UserID, Username, StudentEmail, HashPassword, createdAt, NumReports" + 
                " FROM AppUser WHERE ResetToken = @Token AND ResetTokenExpiry > UTC_TIMESTAMP()";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Token", token);
            using var reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return null; // token doesn't exist, or has expired
            }

            return new User(
                userID: reader.GetInt32("UserID"),
                username: reader.GetString("Username"),
                studentEmail: reader.GetString("StudentEmail"),
                passwordHash: reader.GetString("HashPassword"),
                createdAt: reader.GetDateTime("CreatedAt"),
                numReports: reader.GetInt32("NumReports")
            );
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

        public bool DeleteAccount(User user, string plainPassword) {
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
            command.ExecuteNonQuery();

            return true;
        }

        public List<User> SearchUsers(String usernameOrStudentEmail)
        {
            List<User> users = new List<User>();

            //sql query to search for a user using (a part of) the student number or username
            string sql = "SELECT * FROM AppUser WHERE Username LIKE @Username ? OR StudentEmail LIKE @StudentEmail";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            //execute
            using var command = new MySqlCommand(sql, connection);
            string searchtTerm = "%" + usernameOrStudentEmail + "%";
            command.Parameters.AddWithValue("@StudentEmail", searchtTerm);
            command.Parameters.AddWithValue("@Username", searchtTerm);
            using var reader = command.ExecuteReader();

            if (!reader.Read()){
                return users; // no matching user, by student number or username
            }

            while (reader.NextResult()){
                User user = new User(
                userID: reader.GetInt32("UserID"),
                username: reader.GetString("Username"),
                studentEmail: reader.GetString("StudentEmail"),
                passwordHash: reader.GetString("HashPassword"),
                createdAt: reader.GetDateTime("CreatedAt"),
                numReports: reader.GetInt32("NumReports")
                );
                users.Add(user);
            }

            return users;
        }

        public User? GetUserByID(int userID) {
            //sql query to find user using userID
            string sql = "SELECT * FROM AppUser WHRE UserID = @UserID";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            //execute
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserID", userID);
            using var reader = command.ExecuteReader();

            return new User( //build and return user
                userID: reader.GetInt32("UserID"),
                username: reader.GetString("Username"),
                studentEmail: reader.GetString("StudentEmail"),
                passwordHash: reader.GetString("HashPassword"),
                createdAt: reader.GetDateTime("CreatedAt"),
                numReports: reader.GetInt32("NumReports")
                );
        }

        public User? GetUserByEmail(string studentEmail)
        {
            //sql query to find user using student email
            string sql = "SELECT UserID, Username, StudentEmail, HashPassword, CreatedAt, NumReports " +
                                "FROM AppUser WHERE StudentEmail = @Email";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", studentEmail);
            using var reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            return new User(
                userID: reader.GetInt32("UserID"),
                username: reader.GetString("Username"),
                studentEmail: reader.GetString("StudentEmail"),
                passwordHash: reader.GetString("HashPassword"),
                createdAt: reader.GetDateTime("CreatedAt"),
                numReports: reader.GetInt32("NumReports")
            );
        }

        public void IncrementReportCount(User user)
        {
            //sql query to update the number of reports 
            string sql = "UPDATE AppUser SET Numeports = @NumReports WHERE UserID = @UserID";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            //execute
            int numReports = user.NumReports++;
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@NumReports", numReports);
            command.Parameters.AddWithValue("@UserID", user.UserID);
            command.ExecuteNonQuery();

            //update the object as well
            user.NumReports = numReports;
        }
    }
}
