namespace UJConnect.Models
{
    public class AvailabilitySlot
    {
        public int SlotID {  get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public CampusLocation CampusLocation { get; private set; }
        public User Seller { get; private set; }

        //Constructors
        public AvailabilitySlot(DateTime startTime, DateTime endTime, CampusLocation campusLocation, User seller) {
            StartTime = startTime;
            EndTime = endTime;
            CampusLocation = campusLocation;
            Seller = seller;
        }
        public AvailabilitySlot(int slotID, DateTime startTime, DateTime endTime, CampusLocation campusLocation, User seller)
        {
            SlotID = slotID;
            StartTime = startTime;
            EndTime = endTime;
            CampusLocation = campusLocation;
            Seller = seller;
        }
    }
}
