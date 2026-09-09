namespace UJConnect.Models
{
    public class ClubMembership
    {
        public int MembershipID { get; private set; }
        public Club Club { get; private set; }
        public User Member {  get; private set; }
        public string MemberRole { get; private set; }
        public DateTime JoinedAt { get; private set; }

        //Constructor(s)
        public ClubMembership(Club club, User member) {
            this.Club = club;
            this.Member = member;
            this.MemberRole = "Member";
            this.JoinedAt = DateTime.UtcNow;

            //update capacity of the club
            club.CurrentCapacity++;
        }
        //2nd constr
        public ClubMembership(int membershipID, Club club, User member, string memberRole,DateTime joinedAt)
        {
            this.MembershipID = membershipID;
            this.Club = club;
            this.Member = member;
            this.MemberRole = memberRole;
            this.JoinedAt = joinedAt;

            //update capacity of the club
            club.CurrentCapacity++;
        }

    }
}
