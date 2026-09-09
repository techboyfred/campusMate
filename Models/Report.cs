namespace UJConnect.Models
{
    public class Report
    {
        public int ReportID { get; private set; }
        public User ReportedByUser { get; private set; }
        public User ReportedUser { get; private set; }
        public Product ReportedProduct { get; private set; }
        public string ReportText { get; private set; }
        public DateTime ReportedAt { get; private set; }
        public ReportStatus ReportStatus { get; set; }
        public ReportType ReportType { get; private set; }

        //Constructor for reporting user
        public Report(User reportedByUser, User reportedUser, string reportText, ReportType reportType) {
            this.ReportedByUser = reportedByUser;
            this.ReportedUser = reportedUser;
            this.ReportText = reportText;
            this.ReportStatus = ReportStatus.PENDING;
            this.ReportType = reportType;
            this.ReportedAt = DateTime.UtcNow;
        }
        //Constructor for reporting product (and consequnetly, the seller as well)
        public Report(User reportedByUser, Product reportedProduct, string reportText, ReportType reportType)
        {
            this.ReportedByUser = reportedByUser;
            this.ReportedUser = reportedProduct.AddedByUser;
            this.ReportedProduct = reportedProduct;
            this.ReportText = reportText;
            this.ReportStatus = ReportStatus.PENDING;
            this.ReportType = reportType;
            this.ReportedAt = DateTime.UtcNow;
        }
        //constructor for accessing report
        public Report(int reportID, User reportedByUser, User reportedUser, Product reportedProduct, string reportText, ReportType reportType, ReportStatus reportStatus, DateTime reportedAt)
        {
            this.ReportID = reportID;
            this.ReportedByUser = reportedByUser;
            this.ReportedUser = reportedUser; 
            this.ReportedProduct = reportedProduct;
            this.ReportText = reportText;
            this.ReportStatus = reportStatus;
            this.ReportType = reportType;
            this.ReportedAt = reportedAt;
        }
    }
}
