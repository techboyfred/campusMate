namespace UJConnect.Models
{
    public class Club
    {
        public int ClubID { get; private set; }
        public string Name { get; private set; }
        public decimal RegistrationFee { get; private set; }
        public decimal MembershipFee { get; private set; }
        public int CurrentCapacity { get; set; }
        public int MaxCapacity { get; private set; }
        public DateTime CreatedAt { get; private set; }

        //Constructor(s)
        public Club(string name, decimal registrationFee, decimal membershipFee, int maxCapacity) {
            this.Name = name;
            this.RegistrationFee = registrationFee;
            this.MembershipFee = membershipFee;
            this.CurrentCapacity = 0;
            this.MaxCapacity = maxCapacity;
            this.CreatedAt = DateTime.UtcNow;
        }
        //constr for object from database
        public Club(int clubID, string name, decimal registrationFee, decimal membershipFee, 
            int currentCapacity, int maxCapacity, DateTime createdAt)
        {
            this.ClubID = clubID;
            this.Name = name;
            this.RegistrationFee = registrationFee;
            this.MembershipFee = membershipFee;
            this.CurrentCapacity = currentCapacity;
            this.MaxCapacity = maxCapacity;
            this.CreatedAt = createdAt;
        }
    }
}
