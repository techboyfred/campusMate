namespace UJConnect.Models
{
    public class User
    {
        public int UserID { get; private set; }
        public string Username { get; set; } = string.Empty;
        public string StudentEmail { get; private set; } = string.Empty;

        // Only ever populated when fetching for login verification -
        // never send this back out to the client / never store in session
        public string PasswordHash { get; private set; } = string.Empty;

        public DateTime CreatedAt { get; private set; }
        public int NumReports { get; set; }

        //constructor for registering a new account!!
        public User(string username, String studentEmail, String passwordHash)
        {
            this.Username = username;
            this.StudentEmail = studentEmail;
            this.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordHash);
            this.CreatedAt = DateTime.UtcNow;
            this.NumReports = 0;
        }

        //constructor for logging into an existing account
        public User(int userID, string username, String studentEmail, String passwordHash,
                    DateTime createdAt, int numReports)
        {
            this.UserID = userID;
            this.Username = username;
            this.StudentEmail = studentEmail;
            this.PasswordHash = passwordHash;
            this.CreatedAt = createdAt;
            this.NumReports = numReports;
        }
    }
}
