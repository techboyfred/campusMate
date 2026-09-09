using System.Collections.Generic;

namespace UJConnect.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalStudentCount { get; set; }
        public int ActiveClubCount { get; set; }

        public List<User> FlaggedUsers { get; set; } = new List<User>();
        public List<Report> UnresolvedReports { get; set; } = new List<Report>();
        public List<TutorApplication> PendingTutorApplications { get; set; } = new List<TutorApplication>();
        public List<(int LocationID, string LocationName)> Venues { get; set; } = new List<(int, string)>();
        public List<Club> Clubs { get; set; } = new List<Club>();

        // Only populated when a search was actually performed
        public List<User> SearchResults { get; set; } = new List<User>();
        public string? SearchQuery { get; set; }
    }
}
