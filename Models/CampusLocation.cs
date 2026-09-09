namespace UJConnect.Models
{
    public class CampusLocation
    {
        public int LocationID {  get; private set; }
        public string LocationName {  get; private set; }
        public CampusName CampusName { get; private set; }
        public User AddedByAdmin { get; private set; }

        //Constructor
        public CampusLocation(string locationName, CampusName campusName, User addedByAdmin) {
            AddedByAdmin = addedByAdmin;
            LocationName = locationName;
            CampusName = campusName;
        }
        //constructor for rebuilding object using database info
        public CampusLocation(int locationID, string locationName, CampusName campusName, User addedByAdmin)
        {
            this.LocationID = locationID;
            this.AddedByAdmin = addedByAdmin;
            this.LocationName = locationName;
            this.CampusName = campusName;
        }
    }
}
